using System;
using Project.Combat;
using Project.States;

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
            BattleManager.Instance.OnBattleEnd += ExitCombat;
        }

        public override void Unsubscribe()
        {
            BattleManager.Instance.OnBattleEnd -= ExitCombat;
        }

        private void ExitCombat()
        {
            StateMachine.SwitchState(new WaitForTurnProcess(SuperState, StateMachine));
        }

        public override void Update(float deltaTime)
        {

        }
    }
}