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

        }

        public override void Update(float time)
        {
            GameManager.StartNewTurn();
            StateMachine.SwitchState(new RoundStartResolveState("Round Start Resolve", StateMachine, GameManager));
        }

        public override void OnExit() { }
    }
}