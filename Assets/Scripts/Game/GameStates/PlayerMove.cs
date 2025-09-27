using System;
using Project;
using Project.States;
using UnityEngine;

namespace Project.GameStates
{
    public class PlayerMove : SubState
    {
        public PlayerMove(State superState, StateMachine stateMachine) : base(superState, stateMachine) { }

        public override void Enter() { }

        public override void Exit() { }

        public override void Subscribe()
        {
            GameManager.Instance.Player.InputReader.OnProceedInput += OnProceed;
        }

        public override void Unsubscribe()
        {
            GameManager.Instance.Player.InputReader.OnProceedInput -= OnProceed;
        }


        public override void Update(float deltaTime)
        {
            if (TimeInState > GameManager.Instance.TimeBetweenPlayerMoves)
            {
                Vector2 movementInput = GameManager.Instance.Player.InputReader.MovementValue;
                if (movementInput != Vector2.zero)
                {
                    GameManager.Instance.Player.HeroNode.Move(movementInput);
                    StateMachine.SwitchState(new PlayerMove(new GameRunning(StateMachine), StateMachine));
                }
            }
            if (GameManager.Instance.Hero.MovesRemaining <= 0)
            {
                EndPlayerTurn();
            }
        }

        private void OnProceed()
        {
            EndPlayerTurn();
        }


        private void EndPlayerTurn()
        {
            StateMachine.SwitchState(new TurnProcessing(new GameRunning(StateMachine), StateMachine));
        }
    }
}