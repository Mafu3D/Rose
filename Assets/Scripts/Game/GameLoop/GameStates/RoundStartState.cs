using Project.GameNode;
using Project.GameplayEffects;
using UnityEngine;

namespace Project.GameLoop
{
    public class RoundStartState : State
    {
        public RoundStartState(string name,
                                StateMachine stateMachine,
                                GameManager gameManager) : base(name, stateMachine, gameManager) { }

        public override void OnEnter()
        {
            Debug.Log($"Enter: {Name}");

            foreach (Node node in GameManager.Grid.GetAllRegisteredNodes())
            {
                foreach (GameplayEffectStrategy effect in node.NodeData.OnRoundStartStrategies)
                {
                    GameManager.EffectQueue.AddEffect(effect);
                }
            }
        }

        public override void Update(float time)
        {
            if (TimeInState > GameManager.Instance.MinTimeBetweenPhases)
            {
                StateMachine.SwitchState(new RoundStartResolveState("Round Start Resolve", StateMachine, GameManager));
            }
        }

        public override void OnExit() { }
    }
}