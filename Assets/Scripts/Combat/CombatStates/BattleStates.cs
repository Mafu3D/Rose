

using System;
using System.Collections.Generic;
using Project.States;
using UnityEngine;

namespace Project.Combat.CombatStates
{
    public class BattleStartState : State
    {
        public BattleStartState(string name, StateMachine stateMachine, GameManager gameManager) : base(name, stateMachine, gameManager) { }

        public override void OnEnter()
        {
            GameManager.Instance.Player.InputReader.OnProceedInput += GameManager.BattleManager.ActiveBattle.NextAction;
            GameManager.BattleManager.ActiveBattle.GoToNextState += GoToNexState;

            GameManager.BattleManager.ActiveBattle.StartBattle();
        }

        public override void OnExit()
        {
            GameManager.Instance.Player.InputReader.OnProceedInput -= GameManager.BattleManager.ActiveBattle.NextAction;
            GameManager.BattleManager.ActiveBattle.GoToNextState -= GoToNexState;
        }

        private void GoToNexState()
        {
            StateMachine.SwitchState(new RoundStartState("Round Start", StateMachine, GameManager));
        }

        public override void Update(float deltaTime) { }
    }

    public class RoundStartState : State
    {
        public RoundStartState(string name, StateMachine stateMachine, GameManager gameManager) : base(name, stateMachine, gameManager) { }

        public override void OnEnter()
        {
            GameManager.Instance.Player.InputReader.OnProceedInput += GameManager.BattleManager.ActiveBattle.NextAction;
            GameManager.BattleManager.ActiveBattle.GoToNextState += GoToNexState;

            GameManager.BattleManager.ActiveBattle.StartNewRound();
        }

        public override void OnExit()
        {
            GameManager.Instance.Player.InputReader.OnProceedInput -= GameManager.BattleManager.ActiveBattle.NextAction;
            GameManager.BattleManager.ActiveBattle.GoToNextState -= GoToNexState;
        }

        private void GoToNexState()
        {
            StateMachine.SwitchState(new TurnStartState("Turn Start", StateMachine, GameManager));
        }

        public override void Update(float deltaTime) { }
    }

    public class TurnStartState : State
    {
        public TurnStartState(string name, StateMachine stateMachine, GameManager gameManager) : base(name, stateMachine, gameManager) { }

        public override void OnEnter()
        {
            GameManager.Instance.Player.InputReader.OnProceedInput += GameManager.BattleManager.ActiveBattle.NextAction;
            GameManager.BattleManager.ActiveBattle.GoToNextState += GoToNexState;

            GameManager.BattleManager.ActiveBattle.StartNewTurn();
        }

        public override void OnExit()
        {
            GameManager.Instance.Player.InputReader.OnProceedInput -= GameManager.BattleManager.ActiveBattle.NextAction;
            GameManager.BattleManager.ActiveBattle.GoToNextState -= GoToNexState;
        }

        private void GoToNexState()
        {
            StateMachine.SwitchState(new AttackState("Attack", StateMachine, GameManager));
        }

        public override void Update(float deltaTime) { }
    }

    public class AttackState : State
    {
        public AttackState(string name, StateMachine stateMachine, GameManager gameManager) : base(name, stateMachine, gameManager) { }

        public override void OnEnter()
        {
            GameManager.Instance.Player.InputReader.OnProceedInput += GameManager.BattleManager.ActiveBattle.NextAction;
            GameManager.BattleManager.ActiveBattle.GoToNextState += GoToNexState;

            GameManager.BattleManager.ActiveBattle.DoAttack();
        }

        public override void OnExit()
        {
            GameManager.Instance.Player.InputReader.OnProceedInput -= GameManager.BattleManager.ActiveBattle.NextAction;
            GameManager.BattleManager.ActiveBattle.GoToNextState -= GoToNexState;
        }

        private void GoToNexState()
        {
            StateMachine.SwitchState(new TurnEndState("Turn End", StateMachine, GameManager));
        }

        public override void Update(float deltaTime) { }
    }

    public class TurnEndState : State
    {
        public TurnEndState(string name, StateMachine stateMachine, GameManager gameManager) : base(name, stateMachine, gameManager) { }

        public override void OnEnter()
        {
            GameManager.Instance.Player.InputReader.OnProceedInput += GameManager.BattleManager.ActiveBattle.NextAction;
            GameManager.BattleManager.ActiveBattle.GoToNextState += GoToNexState;

            GameManager.BattleManager.ActiveBattle.EndTurn();
        }

        public override void OnExit()
        {
            GameManager.Instance.Player.InputReader.OnProceedInput -= GameManager.BattleManager.ActiveBattle.NextAction;
            GameManager.BattleManager.ActiveBattle.GoToNextState -= GoToNexState;
        }

        private void GoToNexState()
        {
            if (GameManager.BattleManager.ActiveBattle.Turn == 1)
            {
                StateMachine.SwitchState(new TurnStartState("Turn Start", StateMachine, GameManager));
            }
            else
            {
                StateMachine.SwitchState(new RoundEndState("Round End", StateMachine, GameManager));
            }
        }

        public override void Update(float deltaTime) { }
    }

    public class RoundEndState : State
    {
        public RoundEndState(string name, StateMachine stateMachine, GameManager gameManager) : base(name, stateMachine, gameManager) { }

        public override void OnEnter()
        {
            GameManager.Instance.Player.InputReader.OnProceedInput += GameManager.BattleManager.ActiveBattle.NextAction;
            GameManager.BattleManager.ActiveBattle.GoToNextState += GoToNexState;

            GameManager.BattleManager.ActiveBattle.EndRound();
        }

        public override void OnExit()
        {
            GameManager.Instance.Player.InputReader.OnProceedInput -= GameManager.BattleManager.ActiveBattle.NextAction;
            GameManager.BattleManager.ActiveBattle.GoToNextState -= GoToNexState;
        }

        private void GoToNexState()
        {
            StateMachine.SwitchState(new RoundStartState("Round Start", StateMachine, GameManager));
        }

        public override void Update(float deltaTime) { }
    }

}