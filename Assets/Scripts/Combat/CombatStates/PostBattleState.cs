using System;
using UnityEngine;
using Project.GameLoop;
using Project.States;

namespace Project.Combat.CombatStates
{
    public class PostBattleState : State
    {
        public PostBattleState(string name, StateMachine stateMachine, GameManager gameManager) : base(name, stateMachine, gameManager) { }

        public override void OnEnter()
        {
            GameManager.Player.InputReader.OnProceedInput += GoToNexState;
        }

        public override void OnExit()
        {
            GameManager.Player.InputReader.OnProceedInput -= GoToNexState;
        }

        private void GoToNexState()
        {
            StateMachine.SwitchState(new BattleFinishedState("Battle Finished", StateMachine, GameManager));
        }

        public override void Update(float deltaTime) { }
    }

}