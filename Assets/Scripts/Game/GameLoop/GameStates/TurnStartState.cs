using System.Collections.Generic;
using Project.GameTiles;
using Project.GameplayEffects;
using Project.States;
using UnityEngine;

namespace Project.GameLoop
{
    public class TurnStartState : State
    {
        public TurnStartState(string name,
                                StateMachine stateMachine,
                                GameManager gameManager) : base(name, stateMachine, gameManager) { }

        public override void OnEnter()
        {
            Debug.Log($"Enter: {Name}");

            foreach (Tile tile in GameManager.Grid.GetAllRegisteredTiles())
            {
                tile.ResetForTurn();
                foreach (GameplayEffectStrategy effect in tile.TileData.OnTurnStartStrategies)
                {
                    GameManager.EffectQueue.AddEffect(effect);
                }
            }
        }

        public override void Update(float time)
        {
            if (TimeInState > GameManager.Instance.MinTimeBetweenPhases)
            {
                StateMachine.SwitchState(new TurnStartResolveState("Turn Start Resolve", StateMachine, GameManager));
            }
        }

        public override void OnExit() { }
    }
}