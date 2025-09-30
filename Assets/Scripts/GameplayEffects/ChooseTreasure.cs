using System.Collections.Generic;
using Project.GameLoop;
using Project.Items;
using UnityEngine;

namespace Project.GameplayEffects
{
    [CreateAssetMenu(fileName = "NewChooseTreasure", menuName = "Effects/Choose Treasure", order = 1)]
    public class ChooseTreaure : GameplayEffectStrategy
    {
        public override void ResetEffect()
        {
        }

        public override Status ResolveEffect()
        {
            if (GameManager.Instance.IsChoosingTreasure) return Status.Running;
            return Status.Complete;
        }

        public override Status StartEffect()
        {
            List<Item> choiceItems = GameManager.Instance.ItemDeck.DrawMultiple(3);
            Choice<Item> treasureChoice = new Choice<Item>(choiceItems, EquipChosenItem);
            GameManager.Instance.StartNewTreasureChoice(treasureChoice);
            GameManager.Instance.StateMachine.SwitchState(new SelectingItemState("Selecting Item State", GameManager.Instance.StateMachine, GameManager.Instance));
            return Status.Running;
        }

        private void EquipChosenItem(Item item) {
            switch (item.ItemData.ItemType)
            {
                case ItemType.Weapon:
                    GameManager.Instance.Player.Inventory.SwapEquippedWeapon(item);
                    break;
                default:
                    GameManager.Instance.Player.Inventory.AddItem(item);
                    break;
            }
        }
    }
}