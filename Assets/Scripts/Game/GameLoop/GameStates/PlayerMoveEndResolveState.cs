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
            GameManager.EffectQueue.ResolveQueue();
        }

        public override void Update(float time)
        {
            if (!GameManager.EffectQueue.QueueNeedsToBeResolved)
            {
                GameManager.OnEndOfTurn();
                StateMachine.SwitchState(new EndOfTurnState("End Of Turn", StateMachine, GameManager));
            }
        }

        public override void OnExit() { }
    }
}