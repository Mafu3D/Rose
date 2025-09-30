using System;
using Project.GameNode;
using Project.GameplayEffects;
using UnityEngine;

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
        }

        private void ChooseOptionThree()
        {
            GameManager.ActiveTreasureChoice.ChooseItem(2);
            GameManager.EndTreasureChoice();
        }

        private void ChooseOptionTwo()
        {
            GameManager.ActiveTreasureChoice.ChooseItem(1);
            GameManager.EndTreasureChoice();
        }

        private void ChooseOptionOne()
        {
            GameManager.ActiveTreasureChoice.ChooseItem(0);
            GameManager.EndTreasureChoice();
        }

        public override void Update(float time)
        {
            if (!GameManager.IsChoosingTreasure)
            {
                StateMachine.SwitchState(interruptedState);
            }
        }

        public override void OnExit()
        {
            GameManager.Player.InputReader.OnChoice1Input -= ChooseOptionOne;
            GameManager.Player.InputReader.OnChoice2Input -= ChooseOptionTwo;
            GameManager.Player.InputReader.OnChoice3Input -= ChooseOptionThree;
        }
    }
}