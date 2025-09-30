using UnityEngine;

namespace Project.GameLoop
{
    public class ActivateTilesResolveState : State
    {
        public ActivateTilesResolveState(string name,
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
                GameManager.OnEndOfRound();
                StateMachine.SwitchState(new EndOfRoundState("Round End", StateMachine, GameManager));
            }
        }

        public override void OnExit() { }
    }
}