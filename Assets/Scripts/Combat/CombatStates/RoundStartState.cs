using System;
using UnityEngine;
using Project.GameLoop;

namespace Project.Combat.CombatStates
{
    public class RoundStartState : State
    {
        public RoundStartState(string name, StateMachine stateMachine, GameManager gameManager) : base(name, stateMachine, gameManager) { }

        public override void OnEnter()
        {
            GameManager.BattleManager.ActiveBattle.StartNewRound();
            GameManager.Player.InputReader.OnProceedInput += GameManager.BattleManager.ActiveBattle.NextAction;
            GameManager.BattleManager.ActiveBattle.OnNextActionEvent += Next;
        }

        public override void OnExit()
        {
            GameManager.Player.InputReader.OnProceedInput -= GameManager.BattleManager.ActiveBattle.NextAction;
            GameManager.BattleManager.ActiveBattle.OnNextActionEvent -= Next;
        }

        public override void Update(float deltaTime)
        {

        }

        private void Next()
        {
            if (!GameManager.BattleManager.ActiveBattle.CombatQueue.QueueNeedsToBeResolved)
            {
                StateMachine.SwitchState(new FirstTurnState("First Turn", StateMachine, GameManager));
            }
        }
    }
}