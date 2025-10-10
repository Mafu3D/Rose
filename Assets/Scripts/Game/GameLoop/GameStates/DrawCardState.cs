using System;
using Project.Decks;
using Project.GameTiles;
using Project.GameplayEffects;
using Project.States;
using UnityEngine;
using Project.Core.GameEvents;
using System.Collections.Generic;

namespace Project.GameLoop
{
    public class DrawCardState : State
    {

        TileChoiceEvent tileChoiceEvent;

        public DrawCardState(string name,
                                StateMachine stateMachine,
                                GameManager gameManager) : base(name, stateMachine, gameManager) { }

        public override void OnEnter()
        {
            Debug.Log($"Enter: {Name}");
            GameManager.Player.InputReader.OnProceedInput += Proceed;
            GameManager.Player.InputReader.OnChoice1Input += ChooseOptionOne;
            GameManager.Player.InputReader.OnChoice2Input += ChooseOptionTwo;
            GameManager.Player.InputReader.OnChoice3Input += ChooseOptionThree;
            GameManager.GameEventManager.OnTileDrawEnded += MoveToNextState;

            tileChoiceEvent = GameManager.GameEventManager.StartTileDrawEvent(3, true);
        }

        private void ChooseOptionThree()
        {
            tileChoiceEvent.ChooseItem(2);
            tileChoiceEvent.Resolve();
            GameManager.GameEventManager.EndTileDrawEvent();

        }

        private void ChooseOptionTwo()
        {
            tileChoiceEvent.ChooseItem(1);
            tileChoiceEvent.Resolve();
            GameManager.GameEventManager.EndTileDrawEvent();

        }

        private void ChooseOptionOne()
        {
            tileChoiceEvent.ChooseItem(0);
            tileChoiceEvent.Resolve();
            GameManager.GameEventManager.EndTileDrawEvent();

        }

        private void Proceed()
        {
            if (GameManager.TileDrawManager.ActiveTileChoice.NumberOfChoices == 1)
            {
                tileChoiceEvent.ChooseItem(0);
                tileChoiceEvent.Resolve();
                GameManager.GameEventManager.EndTileDrawEvent();
            }
        }

        public override void Update(float time)
        {
        }

        public void MoveToNextState(IGameEvent _)
        {
            // Cell heroCell = GameManager.Player.HeroTile.CurrentCell;
            // List<Tile> registeredTiles;
            // if (GameManager.Grid.TryGetTileesRegisteredToCell(heroCell, out registeredTiles))
            // {
            //     foreach (Tile tile in registeredTiles)
            //     {

            //         if (tile.CanActivate())
            //         {
            //             tile.ActivatesThisGame += 1;
            //             tile.ActivatesThisTurn += 1;
            //             foreach (GameplayEffectStrategy effect in tile.TileData.OnActivateStrategies)
            //             {
            //                 GameManager.EffectQueue.AddEffect(effect);
            //             }
            //         }
            //     }
            // }
            StateMachine.SwitchState(new DrawCardResolveState("Draw Tile Resolve", StateMachine, GameManager));
        }

        public override void OnExit()
        {
            GameManager.Player.InputReader.OnProceedInput -= Proceed;
            GameManager.Player.InputReader.OnChoice1Input -= ChooseOptionOne;
            GameManager.Player.InputReader.OnChoice2Input -= ChooseOptionTwo;
            GameManager.Player.InputReader.OnChoice3Input -= ChooseOptionThree;

            GameManager.GameEventManager.OnTileDrawEnded -= MoveToNextState;
        }
    }
}