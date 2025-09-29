using UnityEngine;

namespace Project.GameLoop
{
    public class RoundStartResolveState : State
    {
        public RoundStartResolveState(string name,
                                StateMachine stateMachine,
                                GameManager gameManager) : base(name, stateMachine, gameManager) { }

        public override void OnEnter()
        {
            Debug.Log($"Enter: {Name}");
        }

        public override void Update(float time)
        {
            StateMachine.SwitchState(new TurnStartState("Turn Start", StateMachine, GameManager));
        }

        public override void OnExit() { }
    }
}