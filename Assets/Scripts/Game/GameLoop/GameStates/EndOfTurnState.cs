using Project.GameTiles;
using Project.GameplayEffects;
using Project.States;
using UnityEngine;

namespace Project.GameLoop
{
    public class EndOfTurnState : State
    {
        public EndOfTurnState(string name,
                                StateMachine stateMachine,
                                GameManager gameManager) : base(name, stateMachine, gameManager) { }

        public override void OnEnter()
        {
            Debug.Log($"Enter: {Name}");
            foreach (Tile tile in GameManager.Grid.GetAllRegisteredTiles())
            {
                foreach (GameplayEffectStrategy effect in tile.TileData.OnEndOfTurnStrategies)
                {
                    GameManager.EffectQueue.AddEffect(effect);
                }
            }
        }

        public override void Update(float time)
        {
            if (TimeInState > GameManager.Instance.MinTimeBetweenPhases)
            {
                StateMachine.SwitchState(new EndOfTurnResolveState("End Of Turn Resolve", StateMachine, GameManager));
            }
        }

        public override void OnExit() { }
    }
}