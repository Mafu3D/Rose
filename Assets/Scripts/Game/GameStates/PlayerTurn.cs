using System;
using Project.States;

namespace Project.GameStates
{
    public class PlayerTurn : State
    {
        public PlayerTurn(StateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {

        }

        public override void Exit()
        {

        }

        public override void Subscribe()
        {
            GameManager.Instance.EffectQueue.OnResolveQueueStart += GoToResolve;
        }

        public override void Unsubscribe()
        {
            GameManager.Instance.EffectQueue.OnResolveQueueStart -= GoToResolve;
        }

        private void GoToResolve()
        {
            StateMachine.SwitchState(new WaitForTurnProcess(new TurnResolving(StateMachine), StateMachine));
        }

        public override void Update(float deltaTime)
        {

        }
    }
}