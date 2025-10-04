using Project.GameTiles;
using Project.GameplayEffects;
using UnityEngine;

namespace Project.GameLoop
{
    public class EndOfRoundState : State
    {
        public EndOfRoundState(string name,
                                StateMachine stateMachine,
                                GameManager gameManager) : base(name, stateMachine, gameManager) { }

        public override void OnEnter()
        {
            Debug.Log($"Enter: {Name}");
            foreach (Tile tile in GameManager.Grid.GetAllRegisteredTiles())
            {
                foreach (GameplayEffectStrategy effect in tile.TileData.OnEndOfRoundStrategies)
                {
                    GameManager.EffectQueue.AddEffect(effect);
                }
            }
        }

        public override void Update(float time)
        {
            if (TimeInState > GameManager.Instance.MinTimeBetweenPhases)
            {
                StateMachine.SwitchState(new EndOfRoundResolveState("End Of Round Resolve", StateMachine, GameManager));
            }
        }

        public override void OnExit() { }
    }
}