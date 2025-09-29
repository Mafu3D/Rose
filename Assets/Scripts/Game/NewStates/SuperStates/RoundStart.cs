using System;
using Project.States;
using UnityEngine;

namespace Project.GameStates
{
    public class RoundStart : State
    {
        public RoundStart(StateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            GameManager.Instance.Hero.ResetMovesRemaining();
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
            GameManager.Instance.StartPlayerTurnPhase();
            StateMachine.SwitchState(new ResolvingEffects(new PlayerTurn(StateMachine), StateMachine));
        }
    }
}