using System;
using Project.States;
using UnityEngine;

namespace Project.GameStates
{
    public class PlayerTurn : State
    {
        public PlayerTurn(StateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            if (GameManager.Instance.Hero.MovesRemaining <= 0)
            {
                Continue();
            }
        }

        public override void Exit()
        {
        }

        public override void Subscribe()
        {
            GameManager.Instance.Player.InputReader.OnProceedInput += Continue;
        }

        public override void Unsubscribe()
        {
            GameManager.Instance.Player.InputReader.OnProceedInput -= Continue;
        }

        public override void Update(float deltaTime)
        {

        }

        private void Continue()
        {
            GameManager.Instance.StartEndOfTurnPhase();
            StateMachine.SwitchState(new ResolvingEffects(new EndOfTurn(StateMachine), StateMachine));
        }
    }
}