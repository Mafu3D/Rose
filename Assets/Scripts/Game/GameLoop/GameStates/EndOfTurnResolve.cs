using UnityEngine;

namespace Project.GameLoop
{
    public class EndOfTurnResolveState : State
    {
        public EndOfTurnResolveState(string name,
                                StateMachine stateMachine,
                                GameManager gameManager) : base(name, stateMachine, gameManager) { }

        public override void OnEnter()
        {
            Debug.Log($"Enter: {Name}");
        }

        public override void Update(float time)
        {
            StateMachine.SwitchState(new RoundStartState("Round Start", StateMachine, GameManager));
        }

        public override void OnExit() { }
    }
}