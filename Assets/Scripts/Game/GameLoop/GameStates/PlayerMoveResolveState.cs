using UnityEngine;

namespace Project.GameLoop
{
    public class PlayerMoveResolveState : State
    {
        public PlayerMoveResolveState(string name,
                                StateMachine stateMachine,
                                GameManager gameManager) : base(name, stateMachine, gameManager) { }

        public override void OnEnter()
        {
            Debug.Log($"Enter: {Name}");
        }

        public override void Update(float time)
        {
            StateMachine.SwitchState(new EndOfTurnState("End Of Turn", StateMachine, GameManager));
        }

        public override void OnExit() { }
    }
}