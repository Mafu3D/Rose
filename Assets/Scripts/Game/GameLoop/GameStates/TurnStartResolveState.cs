using UnityEngine;
using Project.States;

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
            GameManager.EffectQueue.ResolveQueue();
        }

        public override void Update(float time)
        {
            if (!GameManager.EffectQueue.QueueNeedsToBeResolved)
            {
                GameManager.OnStartPlayerMove();
                StateMachine.SwitchState(new PlayerMoveState("Player Move", StateMachine, GameManager));
            }
        }

        public override void OnExit() { }
    }
}