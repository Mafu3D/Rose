using System;
using Project.States;
using UnityEngine;

namespace Project.GameStates
{
    public class EndOfTurn : State
    {
        public EndOfTurn(StateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            Continue();
        }

        public override void Exit()
        {
        }

        public override void Subscribe()
        {
        }

        public override void Unsubscribe()
        {
        }

        public override void Update(float deltaTime)
        {

        }

        private void Continue()
        {
            GameManager.Instance.StartDrawPhase();
            StateMachine.SwitchState(new ResolvingEffects(new DrawCard(StateMachine), StateMachine));
        }
    }
}