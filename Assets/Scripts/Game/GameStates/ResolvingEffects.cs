using System;
using System.Collections.Generic;
using Project.Combat;
using Project.GameNode;
using Project.States;
using UnityEngine;

namespace Project.GameStates
{
    public class ResolvingEffects : State
    {
        public ResolvingEffects(StateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        public override void Subscribe() {
            GameManager.Instance.EffectQueue.OnResolveQueueEnd += EndResolve;
            GameManager.Instance.OnTresureChoiceStarted += GoToChoice;
         }

        public override void Unsubscribe() {
            GameManager.Instance.EffectQueue.OnResolveQueueEnd -= EndResolve;
            GameManager.Instance.OnTresureChoiceStarted -= GoToChoice;
         }

        private void GoToChoice()
        {
            StateMachine.SwitchState(new Choosing(new ResolvingEffects(StateMachine), StateMachine));
        }

        private void EndResolve()
        {
            StateMachine.SwitchState(new PlayerMove(new PlayerTurn(StateMachine), StateMachine));
        }

        public override void Update(float deltaTime)
        {
        }

        private void EnterCombat()
        {
            StateMachine.SwitchState(new Combat(this, StateMachine));
        }


        private void EnterChoosing()
        {
            StateMachine.SwitchState(new Choosing(this, StateMachine));
        }


    }
}