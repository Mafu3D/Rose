using Project.GameNode;
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
            foreach (Node node in GameManager.Grid.GetAllRegisteredNodes())
            {
                foreach (GameplayEffectStrategy effect in node.NodeData.OnEndOfRoundStrategies)
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