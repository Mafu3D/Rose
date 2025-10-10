using System;
using Project.GameTiles;
using Project.GameplayEffects;
using Project.States;
using UnityEngine;
using Project.Core.GameEvents;
using Project.Items;

namespace Project.GameLoop
{
    public class SelectingItemState : State
    {
        public SelectingItemState(string name,
                                StateMachine stateMachine,
                                GameManager gameManager) : base(name, stateMachine, gameManager) { }

        State interruptedState;

        public override void OnEnter()
        {
            Debug.Log($"Enter: {Name}");
            interruptedState = StateMachine.PreviousState;

            GameManager.Player.InputReader.OnChoice1Input += ChooseOptionOne;
            GameManager.Player.InputReader.OnChoice2Input += ChooseOptionTwo;
            GameManager.Player.InputReader.OnChoice3Input += ChooseOptionThree;
            GameManager.GameEventManager.OnItemDrawEnded += GoBackToInterruptedState;
        }

        private void ChooseOptionThree()
        {
            ItemChoiceEvent itemChoiceEvent = GameManager.GameEventManager.CurrentGameEvent as ItemChoiceEvent;
            itemChoiceEvent.ChooseItem(2);
            itemChoiceEvent.Resolve();
            GameManager.GameEventManager.EndItemDrawEvent();
        }

        private void ChooseOptionTwo()
        {
            ItemChoiceEvent itemChoiceEvent = GameManager.GameEventManager.CurrentGameEvent as ItemChoiceEvent;
            itemChoiceEvent.ChooseItem(1);
            itemChoiceEvent.Resolve();
            GameManager.GameEventManager.EndItemDrawEvent();
        }

        private void ChooseOptionOne()
        {
            ItemChoiceEvent itemChoiceEvent = GameManager.GameEventManager.CurrentGameEvent as ItemChoiceEvent;
            itemChoiceEvent.ChooseItem(0);
            itemChoiceEvent.Resolve();
            GameManager.GameEventManager.EndItemDrawEvent();
        }

        public override void Update(float time)
        {
        }

        private void GoBackToInterruptedState(IGameEvent _)
        {
            StateMachine.SwitchState(interruptedState);
        }

        public override void OnExit()
        {
            GameManager.Player.InputReader.OnChoice1Input -= ChooseOptionOne;
            GameManager.Player.InputReader.OnChoice2Input -= ChooseOptionTwo;
            GameManager.Player.InputReader.OnChoice3Input -= ChooseOptionThree;
            GameManager.GameEventManager.OnItemDrawEnded -= GoBackToInterruptedState;
        }
    }
}