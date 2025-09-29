using System;
using System.Collections.Generic;
using Project.Combat;
using Project.GameNode;
using Project.States;
using UnityEngine;

namespace Project.GameStates
{
    public class ResolvingEffects : SubState
    {
        public ResolvingEffects(State superState, StateMachine stateMachine) : base(superState, stateMachine) { }

        public override void Enter()
        {
            Debug.Log("enter resolving");
            GameManager.Instance.EffectQueue.ResolveQueue(Continue);
        }

        public override void Exit()
        {
        }

        public override void Subscribe() {
            GameManager.Instance.EffectQueue.OnResolveQueueEnd += Continue;
            GameManager.Instance.OnTresureChoiceStarted += GoToChoice;
            BattleManager.Instance.OnNewBattleInitiated += GoToBattle;
         }

        public override void Unsubscribe() {
            GameManager.Instance.EffectQueue.OnResolveQueueEnd -= Continue;
            GameManager.Instance.OnTresureChoiceStarted -= GoToChoice;
            BattleManager.Instance.OnNewBattleInitiated -= GoToBattle;
         }

        private void GoToBattle()
        {
            // StateMachine.SwitchState(new Precombat(new ResolvingEffects(StateMachine), StateMachine));
        }

        private void GoToChoice()
        {
            // StateMachine.SwitchState(new Choosing(new ResolvingEffects(StateMachine), StateMachine));
        }

        private void Continue()
        {
            switch (GameManager.Instance.CurrentPhase)
            {
                case GamePhase.RoundStart:
                    // GameManager.Instance.StartPlayerTurnPhase();
                    StateMachine.SwitchState(new PlayerMove(new PlayerTurn(StateMachine), StateMachine));
                    break;

                case GamePhase.PlayerTurn:
                    // GameManager.Instance.StartEndOfTurnPhase();
                    StateMachine.SwitchState(new NoPlayerInput(new EndOfTurn(StateMachine), StateMachine));
                    break;

                case GamePhase.EndOfTurn:
                    // GameManager.Instance.StartDrawPhase();
                    StateMachine.SwitchState(new NoPlayerInput(new DrawCard(StateMachine), StateMachine));
                    break;

                case GamePhase.Draw:
                    // GameManager.Instance.StartActivateTilesPhase();
                    StateMachine.SwitchState(new NoPlayerInput(new ActivateTiles(StateMachine), StateMachine));
                    break;

                case GamePhase.ActivateTiles:
                    // GameManager.Instance.StartRoundEndPhase();
                    StateMachine.SwitchState(new NoPlayerInput(new RoundEnd(StateMachine), StateMachine));
                    break;

                case GamePhase.RoundEnd:
                    // GameManager.Instance.StartRoundStartPhase();
                    StateMachine.SwitchState(new NoPlayerInput(new RoundStart(StateMachine), StateMachine));
                    break;

                default:
                    StateMachine.SwitchState(new NoPlayerInput(new NoState(StateMachine), StateMachine));
                    break;
            }
        }

        public override void Update(float deltaTime)
        {
        }
    }
}