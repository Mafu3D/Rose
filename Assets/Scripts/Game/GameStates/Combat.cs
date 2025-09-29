using System;
using Project.Combat;
using Project.States;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.GameStates
{
    public class Combat : SubState
    {
        public Combat(State superState, StateMachine stateMachine) : base(superState, stateMachine) { }

        float autoBattleTimer = 0f;

        public override void Enter()
        {

        }

        public override void Exit()
        {

        }

        public override void Subscribe()
        {
            GameManager.Instance.Player.InputReader.OnProceedInput += Proceed;

            BattleManager.Instance.OnBattleConcluded += ExitCombat;
        }

        public override void Unsubscribe()
        {
            GameManager.Instance.Player.InputReader.OnProceedInput -= Proceed;

            BattleManager.Instance.OnBattleConcluded -= ExitCombat;
        }

        private void Proceed()
        {
            if (BattleManager.Instance.IsActiveBattle)
            {
                switch (BattleManager.Instance.Proceed())
                {
                    case Status.Complete:
                        ExitCombat();
                        break;
                    default:
                        break;
                }
            }
        }

        private void ExitCombat()
        {
            StateMachine.SwitchState(new WaitForResolve(new ResolvingEffects(StateMachine), StateMachine));
        }

        public override void Update(float deltaTime)
        {
            if (BattleManager.Instance.IsActiveBattle) {

                if (GameManager.Instance.AutoBattle &&
                    BattleManager.Instance.ActiveBattle.GetPhase() == BattlePhase.Start ||
                    BattleManager.Instance.ActiveBattle.GetPhase() == BattlePhase.FirstTurn ||
                    BattleManager.Instance.ActiveBattle.GetPhase() == BattlePhase.SecondTurn)
                {
                    autoBattleTimer += Time.deltaTime;
                    if (autoBattleTimer > GameManager.Instance.AutoBattleSpeed)
                    {
                        Proceed();
                        autoBattleTimer = 0f;
                    }
                }
            }

        }
    }
}