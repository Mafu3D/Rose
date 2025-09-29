using System;
using UnityEngine;

namespace Project.GameLoop
{
    public class PlayerMoveState : State
    {
        public PlayerMoveState(string name,
                                StateMachine stateMachine,
                                GameManager gameManager) : base(name, stateMachine, gameManager) { }

        float timeInState;

        public override void OnEnter()
        {
            Debug.Log($"Enter: {Name}");

            GameManager.Player.InputReader.OnProceedInput += ProceedEarly;
        }

        public override void Update(float time)
        {
            timeInState += Time.deltaTime;
            if (timeInState > GameManager.TimeBetweenPlayerMoves)
            {
                Vector2 movementInput = GameManager.Player.InputReader.MovementValue;
                if (movementInput != Vector2.zero)
                {
                    GameManager.Player.HeroNode.Move(movementInput);
                }
            }
        }

        public override void OnExit()
        {
            GameManager.Player.InputReader.OnProceedInput -= ProceedEarly;
        }

        private void ProceedEarly()
        {
            GameManager.StartNewEndOfTurn();
            StateMachine.SwitchState(new PlayerMoveResolveState("Player Move Resolve", StateMachine, GameManager));
        }
    }
}