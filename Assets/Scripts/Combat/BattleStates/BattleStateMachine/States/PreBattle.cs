
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Combat.BattleStateMachine
{
    public class PreBattle : State
    {
        public PreBattle(StateMachine stateMachine, Battle battle) : base(stateMachine, battle) { }

        Choice<Action> PreBattleChoice;

        public override void Enter()
        {
            List<Action> prebattleChoices = new List<Action> { ChooseFight, ChooseSteal, ChooseRun };
            PreBattleChoice = new Choice<Action>(prebattleChoices, ResolvePrebattleChoice);
        }

        public override void Exit() { }

        public override void Subscribe() {
            GameManager.Instance.Player.InputReader.OnChoice1Input += OnChooseOne;
            GameManager.Instance.Player.InputReader.OnChoice2Input += OnChooseTwo;
            GameManager.Instance.Player.InputReader.OnChoice3Input += OnChooseThree;
         }

        public override void Unsubscribe() {
            GameManager.Instance.Player.InputReader.OnChoice1Input -= OnChooseOne;
            GameManager.Instance.Player.InputReader.OnChoice2Input -= OnChooseTwo;
            GameManager.Instance.Player.InputReader.OnChoice3Input -= OnChooseThree;
         }

        public override void Update(float deltaTime) { }

        private void OnChooseOne() => PreBattleChoice.ChooseItem(0);
        private void OnChooseTwo() => PreBattleChoice.ChooseItem(1);
        private void OnChooseThree() => PreBattleChoice.ChooseItem(2);
        private void ResolvePrebattleChoice(Action action) => action();

        private void ChooseRun()
        {
            Debug.Log("I choose to run!");
            StateMachine.SwitchState(new AttemptToRun(StateMachine, Battle));
        }

        private void ChooseSteal()
        {
            Debug.Log("I choose to steal!");
            StateMachine.SwitchState(new AttemptToSteal(StateMachine, Battle));
        }

        private void ChooseFight()
        {
            Debug.Log("I choose to fight!");
            StateMachine.SwitchState(new BattleStarting(StateMachine, Battle));
        }
    }

}