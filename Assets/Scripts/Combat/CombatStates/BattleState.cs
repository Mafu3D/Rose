

using System;
using System.Collections.Generic;
using Project.GameLoop;
using UnityEngine;

namespace Project.Combat.CombatStates
{
    public class BattleState : State
    {
        public BattleState(string name, StateMachine stateMachine, GameManager gameManager) : base(name, stateMachine, gameManager) { }

        float timer = 3f;
        float time;

        public override void OnEnter()
        {
            GameManager.Instance.Player.InputReader.OnProceedInput += GameManager.BattleManager.ActiveBattle.NextAction;

            GameManager.BattleManager.ActiveBattle.BattleState = Combat.BattleState.BattleStart;
        }

        public override void OnExit()
        {
            GameManager.Instance.Player.InputReader.OnProceedInput -= GameManager.BattleManager.ActiveBattle.NextAction;

         }

        public override void Update(float deltaTime)
        {
            time += Time.deltaTime;
            if (time > timer)
            {
                //pass
            }
        }
    }

}