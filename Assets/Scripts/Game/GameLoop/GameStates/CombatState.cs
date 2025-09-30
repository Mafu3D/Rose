using Project.GameNode;
using Project.GameplayEffects;
using UnityEngine;

namespace Project.GameLoop
{
    public class CombatState : State
    {
        public CombatState(string name,
                                StateMachine stateMachine,
                                GameManager gameManager) : base(name, stateMachine, gameManager) { }

        State interruptedState;

        public override void OnEnter()
        {
            Debug.Log($"Enter: {Name}");
            interruptedState = StateMachine.PreviousState;

            // foreach (Node node in GameManager.Grid.GetAllRegisteredNodes())
            // {
            //     foreach (GameplayEffectStrategy effect in node.NodeData.OnEndOfRoundStrategies)
            //     {
            //         GameManager.EffectQueue.AddEffect(effect);
            //     }
            // }
        }

        public override void Update(float time)
        {
            if (TimeInState > 5f)
            {
                StateMachine.SwitchState(interruptedState);
            }
        }

        public override void OnExit() { }
    }
}