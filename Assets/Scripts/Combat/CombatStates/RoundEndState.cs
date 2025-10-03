using System;
using UnityEngine;
using Project.GameLoop;

namespace Project.Combat.CombatStates
{
    public class RoundEndState : State
    {
        public RoundEndState(string name, StateMachine stateMachine, GameManager gameManager) : base(name, stateMachine, gameManager) { }

        public override void OnEnter()
        {
            GameManager.BattleManager.ActiveBattle.EndRound();
            GameManager.Player.InputReader.OnProceedInput += GameManager.BattleManager.ActiveBattle.NextAction;
        }

        public override void OnExit()
        {
            GameManager.Player.InputReader.OnProceedInput -= GameManager.BattleManager.ActiveBattle.NextAction;
        }

        public override void Update(float deltaTime)
        {
            if (!GameManager.BattleManager.ActiveBattle.CombatQueue.QueueNeedsToBeResolved)
            {
                StateMachine.SwitchState(new RoundStartState("Round Start", StateMachine, GameManager));
            }
        }
    }
}