using UnityEngine;

namespace Project.GameLoop
{
    public class DrawCardResolveState : State
    {
        public DrawCardResolveState(string name,
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
                StateMachine.SwitchState(new EndOfRoundState("Round End", StateMachine, GameManager));
            }
        }

        public override void OnExit() { }
    }
}