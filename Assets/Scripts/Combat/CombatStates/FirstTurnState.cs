using System;
using UnityEngine;
using Project.GameLoop;

namespace Project.Combat.CombatStates
{
    public class FirstTurnState : State
    {
        public FirstTurnState(string name, StateMachine stateMachine, GameManager gameManager) : base(name, stateMachine, gameManager) { }

        bool attackFired = false;

        public override void OnEnter()
        {
            GameManager.BattleManager.ActiveBattle.StartNewTurn();
            GameManager.BattleManager.ActiveBattle.OnAttackEnd += MoveToNextState;
            GameManager.Player.InputReader.OnProceedInput += GameManager.BattleManager.ActiveBattle.NextAction;
            GameManager.BattleManager.ActiveBattle.OnNextActionEvent += Next;
        }

        public override void OnExit()
        {
            GameManager.BattleManager.ActiveBattle.OnAttackEnd -= MoveToNextState;
            GameManager.Player.InputReader.OnProceedInput -= GameManager.BattleManager.ActiveBattle.NextAction;
            GameManager.BattleManager.ActiveBattle.OnNextActionEvent -= Next;
        }

        private void Next()
        {
            if (!GameManager.BattleManager.ActiveBattle.CombatQueue.QueueNeedsToBeResolved && !attackFired)
            {
                GameManager.BattleManager.ActiveBattle.DoAttack();
                attackFired = true;
            }
        }

        public override void Update(float deltaTime)
        {

        }

        private void MoveToNextState()
        {
            StateMachine.SwitchState(new SecondTurnState("Second Turn", StateMachine, GameManager));
        }
    }

}