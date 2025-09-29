using System;
using Project.Combat;
using Project.States;
using UnityEngine.InputSystem;

namespace Project.GameStates
{
    public class Combat : SubState
    {
        public Combat(State superState, StateMachine stateMachine) : base(superState, stateMachine) { }

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

        }
    }
}