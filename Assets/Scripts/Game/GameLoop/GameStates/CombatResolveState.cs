using Project.States;
using UnityEngine;

namespace Project.GameLoop
{
    public class CombatResolveState : State
    {
        public CombatResolveState(string name,
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
                GameManager.OnNewRound();
                StateMachine.SwitchState(new RoundStartState("Round Start", StateMachine, GameManager));
            }
        }

        public override void OnExit() { }
    }
}