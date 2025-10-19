

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



        public override void OnEnter()
        {
            GameManager.Instance.Player.InputReader.OnNumInput += Choose;
        }

        public override void OnExit()
        {
            GameManager.Instance.Player.InputReader.OnNumInput -= Choose;
        }

        public override void Update(float deltaTime) { }

        private void Choose(int num)
        {
            Battle activeBattle = GameManager.Instance.BattleManager.ActiveBattle;
            if (num > activeBattle.PreBattleChoice.NumberOfChoices) return;
            activeBattle.PreBattleChoice.ChooseItem(num-1);
            activeBattle.PreBattleChoice.Resolve();
        }
    }

}