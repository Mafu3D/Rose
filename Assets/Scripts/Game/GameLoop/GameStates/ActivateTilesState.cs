using System.Collections.Generic;
using Project.GameTiles;
using Project.GameplayEffects;
using Project.States;
using UnityEngine;

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
                StateMachine.SwitchState(new DrawCardState("Draw Tile", StateMachine, GameManager));
            }
        }

        public override void Update(float time)
        {
        }

        public override void OnExit() { }
    }
}