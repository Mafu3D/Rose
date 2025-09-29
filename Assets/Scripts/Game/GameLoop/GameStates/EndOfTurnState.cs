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
        }

        public override void Update(float time)
        {
            GameManager.StartNewRound();
            StateMachine.SwitchState(new EndOfTurnResolveState("End Of Turn Resolve", StateMachine, GameManager));
        }

        public override void OnExit() { }
    }
}