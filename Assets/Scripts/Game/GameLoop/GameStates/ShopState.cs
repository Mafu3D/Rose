using System;
using Project.GameTiles;
using Project.GameplayEffects;
using Project.States;
using UnityEngine;
using Project.Core.GameEvents;
using Project.Items;

namespace Project.GameLoop
{
    public class ShopState : State
    {
        public ShopState(string name,
                                StateMachine stateMachine,
                                GameManager gameManager) : base(name, stateMachine, gameManager) { }

        State interruptedState;
        ChoiceEvent<ItemData> choiceEvent;

        public override void OnEnter()
        {
            Debug.Log($"Enter: {Name}");
            interruptedState = StateMachine.PreviousState;

            GameManager.Player.InputReader.OnNumInput += Choose;
            GameManager.Player.InputReader.OnExitInput += Exit;
            GameManager.GameEventManager.OnShopEnded += GoBackToInterruptedState;

            choiceEvent = GameManager.GameEventManager.CurrentShopEvent;
        }

        private void Choose(int num)
        {
            if (num == 0)
            {
                RefreshShop();
                return;
            }

            if (num > choiceEvent.Choice.NumberOfChoices) return;

            if (GameManager.Hero.Character.Inventory.InventoryIsFull)
            {
                CalloutUI.Instance.QueueCallout("Inventory full! Please discard an item by left clicking it.");
                return;
            }

            choiceEvent.ChooseItem(num - 1);
            choiceEvent.Resolve();
        }

        private void RefreshShop()
        {
            ShopEvent shopEvent = choiceEvent as ShopEvent;
            shopEvent.Refresh();
        }

        private void Exit()
        {
            if (choiceEvent.IsExitable)
            {
                choiceEvent.Resolve();
                GameManager.GameEventManager.EndShopEvent();
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
            GameManager.GameEventManager.OnShopEnded -= GoBackToInterruptedState;
        }
    }
}