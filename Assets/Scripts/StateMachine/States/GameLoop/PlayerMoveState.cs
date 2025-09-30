using System;
using Project.States;
using UnityEngine;

namespace Project.States
{
    public class PlayerMoveState : BaseState
    {
        public PlayerMoveState(string name, GameManager gameManager) : base(name, gameManager) { }

        float timeInState;

        public override void OnEnter()
        {
            Debug.Log($"Enter: {Name}");
            // GameManager.PhaseSwitch.StartNewPhase();

            // GameManager.PhaseSwitch.CompletePhase();
            GameManager.StartNewEndOfTurn();
            return;

            GameManager.Player.InputReader.OnProceedInput += ProceedEarly;
        }

        public override void Update()
        {
            timeInState += Time.deltaTime;
            if (timeInState > GameManager.MinTimeBetweenPlayerMoves)
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
            // GameManager.PhaseSwitch.CompletePhase();
        }
    }
}