using Project.States;
using UnityEngine;
using Project.Core.GameEvents;
using Project.Items;

namespace Project.GameLoop
{
    public class DiscardingItemState : State
    {
        public DiscardingItemState(string name,
                                StateMachine stateMachine,
                                GameManager gameManager) : base(name, stateMachine, gameManager) { }

        State interruptedState;
        ChoiceEvent<Item> choiceEvent;

        public override void OnEnter()
        {
            Debug.Log($"Enter: {Name}");
            interruptedState = StateMachine.PreviousState;

            GameManager.Player.InputReader.OnNumInput += Choose;
            GameManager.Player.InputReader.OnExitInput += Exit;
            GameManager.GameEventManager.OnInventoryChoiceEnded += GoBackToInterruptedState;

            choiceEvent = GameManager.GameEventManager.CurrentInventoryChoiceEvent;
        }

        private void Choose(int num)
        {
            if (num > choiceEvent.Choice.NumberOfChoices) return;

            if (num == 0 && choiceEvent.Choice.GetAllItems()[0] != null)
            {
                choiceEvent.ChooseItem(0);
            }
            else if (num == 9 && choiceEvent.Choice.GetAllItems()[9] != null)
            {
                choiceEvent.ChooseItem(9);
            }
            else if (choiceEvent.Choice.GetAllItems()[num - 1] != null)
            {
                choiceEvent.ChooseItem(num - 1);
            }
        }

        private void Exit()
        {
            if (choiceEvent.IsExitable)
            {
                choiceEvent.Resolve();
                // GameManager.GameEventManager.EndInventoryChoiceEvent();
            }
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
            GameManager.Player.InputReader.OnNumInput -= Choose;
            GameManager.Player.InputReader.OnExitInput -= Exit;
            GameManager.GameEventManager.OnInventoryChoiceEnded -= GoBackToInterruptedState;
        }
    }
}