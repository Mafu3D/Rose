using System;
using System.Collections.Generic;
using Project.GameTiles;
using Project.GameplayEffects;
using UnityEngine;
using Project.States;

namespace Project.GameLoop
{
    public class PlayerMoveState : State
    {
        public PlayerMoveState(string name,
                                StateMachine stateMachine,
                                GameManager gameManager) : base(name, stateMachine, gameManager) { }

        float timeInState;

        public override void OnEnter()
        {
            Debug.Log($"Enter: {Name}");

            GameManager.Player.InputReader.OnProceedInput += Proceed;

            if (GameManager.Player.HeroTile.MovesRemaining == 0)
            {
                foreach (Tile node in GameManager.Grid.GetAllRegisteredTiles())
                {
                    foreach (GameplayEffectStrategy effect in node.TileData.OnPlayerMoveEndStrategies)
                    {
                        GameManager.EffectQueue.AddEffect(effect);
                    }
                }
                Proceed();
            }
        }

        public override void Update(float time)
        {
            timeInState += Time.deltaTime;
            if (timeInState > GameManager.MinTimeBetweenPlayerMoves)
            {
                Vector2 movementInput = GameManager.Player.InputReader.MovementValue;
                if (movementInput != Vector2.zero)
                {
                    Cell heroCell = GameManager.Player.HeroTile.CurrentCell;
                    List<Tile> registeredTiles;
                    if (GameManager.Grid.TryGetTileesRegisteredToCell(heroCell, out registeredTiles))
                    {
                        foreach (Tile tile in registeredTiles)
                        {
                            if (tile.CanPlayerExit())
                            {
                                tile.PlayerExitsThisGame += 1;
                                tile.PlayerExitsThisTurn += 1;
                                foreach (GameplayEffectStrategy effect in tile.TileData.OnPlayerExitStrategies)
                                {
                                    GameManager.EffectQueue.AddEffect(effect);
                                }
                            }
                        }
                    }


                    GameManager.Player.HeroTile.Move(movementInput);
                    GameManager.OnPlayerMove();
                    Resolve();
                }
            }
        }

        public override void OnExit()
        {
            GameManager.Player.InputReader.OnProceedInput -= Proceed;
        }

        private void Proceed()
        {
            StateMachine.SwitchState(new PlayerMoveEndResolveState("Player Move End Resolve", StateMachine, GameManager));
        }

        private void Resolve()
        {
            foreach (Tile tile in GameManager.Grid.GetAllRegisteredTiles())
            {
                foreach (GameplayEffectStrategy effect in tile.TileData.OnPlayerMoveStrategies)
                {
                    GameManager.EffectQueue.AddEffect(effect);
                }
            }

            Cell heroCell = GameManager.Player.HeroTile.CurrentCell;
            List<Tile> registeredTiles;
            if (GameManager.Grid.TryGetTileesRegisteredToCell(heroCell, out registeredTiles))
            {
                foreach (Tile tile in registeredTiles)
                {
                    if (tile.CanPlayerEnter())
                    {
                        tile.PlayerEntersThisGame += 1;
                        tile.PlayerEntersThisTurn += 1;
                        foreach (GameplayEffectStrategy effect in tile.TileData.OnPlayerEnterStrategies)
                        {
                            GameManager.EffectQueue.AddEffect(effect);
                        }
                    }
                }

            }
            StateMachine.SwitchState(new PlayerMoveResolveState("Player Move Resolve", StateMachine, GameManager));
        }
    }
}