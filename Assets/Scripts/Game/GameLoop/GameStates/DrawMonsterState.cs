using System.Collections.Generic;
using Project.Core.GameEvents;
using Project.GameTiles;
using Project.States;
using UnityEngine;

namespace Project.GameLoop
{
    public class DrawMonsterState : State
    {
        CardChoiceEvent monsterChoiceEvent;

        public DrawMonsterState(string name, StateMachine stateMachine, GameManager gameManager) : base(name, stateMachine, gameManager) { }

        public override void OnEnter()
        {
            Debug.Log($"Enter: {Name}");
            GameManager.Player.InputReader.OnProceedInput += Proceed;

            DangerStatus dangerStatus = DangerStatus.Standard;

            // You're checking the hero cell three times per round!!!!
            Cell heroCell = GameManager.Player.HeroTile.CurrentCell;
            List<Tile> registeredTiles;
            if (GameManager.Grid.TryGetTileesRegisteredToCell(heroCell, out registeredTiles))
            {
                foreach (Tile tile in registeredTiles)
                {
                    if (tile.TileData.DangerStatus == DangerStatus.Elite)
                    {
                        dangerStatus = DangerStatus.Elite;
                        break;
                    }
                    if (tile.TileData.DangerStatus == DangerStatus.Dangerous)
                    {
                        dangerStatus = DangerStatus.Dangerous;
                        break;
                    }
                    if (tile.TileData.DangerStatus == DangerStatus.Safe)
                    {
                        dangerStatus = DangerStatus.Safe;
                        break;
                    }
                }
            }

            switch (dangerStatus)
            {
                case DangerStatus.Standard:
                    float random = Random.Range(0f, 100f);
                    if (random <= GameManager.ChanceForMonsterEncounter) monsterChoiceEvent = GameManager.GameEventManager.StartCardDrawEvent(GameManager.MonsterDeck, 1, false);
                    else StateMachine.SwitchState(new ActivateTilesState("Activate Tiles", StateMachine, GameManager));
                    break;
                case DangerStatus.Safe:
                    StateMachine.SwitchState(new ActivateTilesState("Activate Tiles", StateMachine, GameManager));
                    break;
                case DangerStatus.Dangerous:
                    monsterChoiceEvent = GameManager.GameEventManager.StartCardDrawEvent(GameManager.MonsterDeck, 1, false);
                    break;
                case DangerStatus.Elite:
                    monsterChoiceEvent = GameManager.GameEventManager.StartCardDrawEvent(GameManager.EliteMonsterDeck, 1, false);
                    break;
            }
            if (monsterChoiceEvent != null)
            {
                GameManager.GameEventManager.OnCardDrawEnded += GoToNextState;
            }
        }

        public override void Update(float time)
        {
        }

        public override void OnExit()
        {
            GameManager.Player.InputReader.OnProceedInput -= Proceed;
        }

        private void Proceed()
        {
            monsterChoiceEvent.ChooseItem(0);
            monsterChoiceEvent.Resolve();
            GameManager.GameEventManager.EndCardDrawEvent();
        }

        private void GoToNextState(IGameEvent gameEvent)
        {
            GameManager.GameEventManager.OnCardDrawEnded -= GoToNextState;
            StateMachine.SwitchState(new DrawMonsterResolveState("Draw Monster Resolve", StateMachine, GameManager));
        }
    }
}