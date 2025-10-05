using UnityEngine;
using Project.States;

namespace Project.GameLoop
{
    public class EndOfTurnResolveState : State
    {
        public EndOfTurnResolveState(string name,
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
                StateMachine.SwitchState(new ActivateTilesState("Activate Tiles", StateMachine, GameManager));
            }
        }

        public override void OnExit() { }
    }
}