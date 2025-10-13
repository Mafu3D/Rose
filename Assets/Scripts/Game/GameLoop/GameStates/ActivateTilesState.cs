using System.Collections.Generic;
using Project.GameTiles;
using Project.GameplayEffects;
using Project.States;
using UnityEngine;
using Project.Combat.CombatStates;

namespace Project.GameLoop
{
    public class ActivateTilesState : State
    {
        public ActivateTilesState(string name,
                                StateMachine stateMachine,
                                GameManager gameManager) : base(name, stateMachine, gameManager) { }

        public override void OnEnter()
        {
            Debug.Log($"Enter: {Name}");
            // You're checking the hero cell twice!

            if (GameManager.Instance.WillActivateTilesThisTurn)
            {
                Cell heroCell = GameManager.Player.HeroTile.CurrentCell;
                List<Tile> registeredTiles;
                if (GameManager.Grid.TryGetTileesRegisteredToCell(heroCell, out registeredTiles))
                {
                    foreach (Tile tile in registeredTiles)
                    {

                        if (tile.CanActivate())
                        {
                            tile.ActivatesThisGame += 1;
                            tile.ActivatesThisTurn += 1;
                            foreach (GameplayEffectStrategy effect in tile.TileData.OnActivateStrategies)
                            {
                                GameManager.EffectQueue.AddEffect(effect);
                            }
                        }
                    }
                    StateMachine.SwitchState(new ActivateTilesResolveState("Activate Tiles Resolve", StateMachine, GameManager));
                }
                else
                {
                    StateMachine.SwitchState(new EndOfRoundState("Round End", StateMachine, GameManager));
                }
            }
            else
            {
                StateMachine.SwitchState(new EndOfRoundState("Round End", StateMachine, GameManager));
            }
        }

        public override void Update(float time)
        {
        }

        public override void OnExit() { }
    }
}