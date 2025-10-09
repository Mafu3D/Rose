

using System;
using System.Collections.Generic;
using Project.States;
using Project.Combat.CombatStates;
using UnityEngine;

namespace Project.Combat.CombatStates
{
    public class PreBattleState : State
    {
        public PreBattleState(string name, StateMachine stateMachine, GameManager gameManager) : base(name, stateMachine, gameManager) { }

        Choice<Action> PreBattleChoice;

        public override void OnEnter()
        {
            List<Action> prebattleChoices = new List<Action> { ChooseFight, ChooseSteal, ChooseRun };
            PreBattleChoice = new Choice<Action>(prebattleChoices, ResolvePrebattleChoice);

            GameManager.Instance.Player.InputReader.OnChoice1Input += OnChooseOne;
            GameManager.Instance.Player.InputReader.OnChoice2Input += OnChooseTwo;
            GameManager.Instance.Player.InputReader.OnChoice3Input += OnChooseThree;
        }

        public override void OnExit()
        {
            GameManager.Instance.Player.InputReader.OnChoice1Input -= OnChooseOne;
            GameManager.Instance.Player.InputReader.OnChoice2Input -= OnChooseTwo;
            GameManager.Instance.Player.InputReader.OnChoice3Input -= OnChooseThree;
         }

        public override void Update(float deltaTime) { }

        private void OnChooseOne() => PreBattleChoice.ChooseItem(0);
        private void OnChooseTwo() => PreBattleChoice.ChooseItem(1);
        private void OnChooseThree() => PreBattleChoice.ChooseItem(2);
        private void ResolvePrebattleChoice(Action action, List<Action> _) => action();

        private void ChooseRun()
        {
            Debug.Log("I choose to run!");
            StateMachine.SwitchState(new AttemptToRunState("Attempt to Run", StateMachine, GameManager));
        }

        private void ChooseSteal()
        {
            Debug.Log("I choose to steal!");
            StateMachine.SwitchState(new AttemptToStealState("Attempt To Steal", StateMachine, GameManager));
        }

        private void ChooseFight()
        {
            Debug.Log("I choose to fight!");
            StateMachine.SwitchState(new BattleStartState("Battle State", StateMachine, GameManager));
        }
    }

}