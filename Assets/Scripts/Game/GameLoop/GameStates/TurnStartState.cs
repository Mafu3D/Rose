using UnityEngine;

namespace Project.GameLoop
{
    public class TurnStartState : State
    {
        public TurnStartState(string name,
                                StateMachine stateMachine,
                                GameManager gameManager) : base(name, stateMachine, gameManager) { }

        public override void OnEnter()
        {
            Debug.Log($"Enter: {Name}");

        }

        public override void Update(float time)
        {
            GameManager.StartNewPlayerMove();
            StateMachine.SwitchState(new TurnStartResolveState("Turn Start Resolve", StateMachine, GameManager));
        }

        public override void OnExit() { }
    }
}