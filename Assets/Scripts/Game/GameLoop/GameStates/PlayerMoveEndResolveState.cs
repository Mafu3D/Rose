using UnityEngine;

namespace Project.GameLoop
{
    public class PlayerMoveEndResolveState : State
    {
        public PlayerMoveEndResolveState(string name,
                                StateMachine stateMachine,
                                GameManager gameManager) : base(name, stateMachine, gameManager) { }

        public override void OnEnter()
        {
            Debug.Log($"Enter: {Name}");
        }

        public override void Update(float time)
        {
        }

        public override void OnExit() { }
    }
}