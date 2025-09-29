using UnityEngine;

namespace Project.GameLoop
{
    public class TurnStartResolveState : State
    {
        public TurnStartResolveState(string name,
                                StateMachine stateMachine,
                                GameManager gameManager) : base(name, stateMachine, gameManager) { }

        public override void OnEnter()
        {
            Debug.Log($"Enter: {Name}");
        }

        public override void Update(float time)
        {
            StateMachine.SwitchState(new PlayerMoveState("Player Move", StateMachine, GameManager));
        }

        public override void OnExit() { }
    }
}