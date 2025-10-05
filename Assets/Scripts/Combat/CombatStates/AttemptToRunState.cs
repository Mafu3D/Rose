using System;
using UnityEngine;
using Project.GameLoop;
using Project.States;

namespace Project.Combat.CombatStates
{
    public class AttemptToRunState : State
    {
        public AttemptToRunState(string name, StateMachine stateMachine, GameManager gameManager) : base(name, stateMachine, gameManager) { }

        public override void OnEnter()
        {
            GameManager.Player.InputReader.OnProceedInput += NextActionManual;
            GameManager.BattleManager.AutoTimerTick += NextActionAuto;
            GameManager.BattleManager.ActiveBattle.GoToNextState += GoToNextState;

            GameManager.BattleManager.ActiveBattle.RunAway();
        }

        public override void OnExit()
        {
            GameManager.Player.InputReader.OnProceedInput -= NextActionManual;
            GameManager.BattleManager.AutoTimerTick -= NextActionAuto;
            GameManager.BattleManager.ActiveBattle.GoToNextState -= GoToNextState;

        }

        private void NextActionManual() { if (!GameManager.AutoBattle) GameManager.BattleManager.ActiveBattle.NextAction(); }
        private void NextActionAuto() { if (GameManager.AutoBattle) GameManager.BattleManager.ActiveBattle.NextAction(); }

        private void GoToNextState()
        {
            StateMachine.SwitchState(new PostBattleState("Post Battle", StateMachine, GameManager));
        }


        public override void Update(float deltaTime) { }
    }

}