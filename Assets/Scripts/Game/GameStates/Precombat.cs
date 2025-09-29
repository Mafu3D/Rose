using System;
using Project.Combat;
using Project.States;
using UnityEngine.InputSystem;

namespace Project.GameStates
{
    public class Precombat : SubState
    {
        public Precombat(State superState, StateMachine stateMachine) : base(superState, stateMachine) { }

        public override void Enter()
        {

        }

        public override void Exit()
        {

        }

        public override void Subscribe()
        {
            GameManager.Instance.Player.InputReader.OnChoice1Input += ChooseOne;
            GameManager.Instance.Player.InputReader.OnChoice2Input += ChooseTwo;
            GameManager.Instance.Player.InputReader.OnChoice3Input += ChooseThree;

            BattleManager.Instance.OnBattleConcluded += ExitCombat;
        }

        public override void Unsubscribe()
        {
            GameManager.Instance.Player.InputReader.OnChoice1Input -= ChooseOne;
            GameManager.Instance.Player.InputReader.OnChoice2Input -= ChooseTwo;
            GameManager.Instance.Player.InputReader.OnChoice3Input -= ChooseThree;

            BattleManager.Instance.OnBattleConcluded -= ExitCombat;
        }


        private void ChooseOne()
        {
            if (BattleManager.Instance.ActiveBattle.PreBattleChoice != null)
            {
                BattleManager.Instance.ActiveBattle.PreBattleChoice.ChooseItem(0);
                StateMachine.SwitchState(new Combat(new ResolvingEffects(StateMachine), StateMachine));
            }
        }

        private void ChooseTwo()
        {
            if (BattleManager.Instance.ActiveBattle.PreBattleChoice != null)
            {
                BattleManager.Instance.ActiveBattle.PreBattleChoice.ChooseItem(1);
                StateMachine.SwitchState(new Combat(new ResolvingEffects(StateMachine), StateMachine));
            }
        }

        private void ChooseThree()
        {
            if (BattleManager.Instance.ActiveBattle.PreBattleChoice != null)
            {
                BattleManager.Instance.ActiveBattle.PreBattleChoice.ChooseItem(2);
                StateMachine.SwitchState(new Combat(new ResolvingEffects(StateMachine), StateMachine));
            }
        }

        private void ExitCombat()
        {
            StateMachine.SwitchState(new WaitForResolve(new ResolvingEffects(StateMachine), StateMachine));
        }

        public override void Update(float deltaTime)
        {
            if (!BattleManager.Instance.IsActiveBattle) ExitCombat();
        }
    }
}