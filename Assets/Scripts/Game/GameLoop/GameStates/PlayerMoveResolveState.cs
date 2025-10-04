using UnityEngine;
using Project.States;

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
            GameManager.EffectQueue.ResolveQueue();
        }

        public override void Update(float time)
        {
            StateMachine.SwitchState(new PlayerMoveState("Player Move", StateMachine, GameManager));
        }

        public override void OnExit() { }
    }
}