using System.Collections.Generic;
using Project.Items;

namespace Project.Core.GameEvents
{
    public class ItemChoiceEvent : ChoiceEvent<ItemData>
    {
        private bool shuffleRemaining;
        public ItemChoiceEvent(int amount, bool shuffleRemaining) : base(amount)
        {
            this.shuffleRemaining = shuffleRemaining;
        }

        public override void GenerateChoices()
        {
            List<ItemData> choices = GameManager.Instance.ItemDeck.DrawMultiple(amount);
            if (choices.Count == 0) return;
            Choice = new Choice<ItemData>(choices, ResolveCallback);
        }

        public override void ChooseItem(int index)
        {
            Choice.ChooseItem(index);
        }

        protected override void ResolveCallback(List<ItemData> chosen, List<ItemData> notChosen)
        {
            foreach(ItemData itemData in chosen)
            {
                Item item = new Item(itemData);
                switch (itemData.ItemType)
                {
                    case ItemType.Weapon:
                        GameManager.Instance.Player.Inventory.SwapEquippedWeapon(item);
                        break;
                    default:
                        GameManager.Instance.Player.Inventory.AddItem(item);
                        break;
                }
            }

            if (shuffleRemaining)
            {
                GameManager.Instance.ItemDeck.AddToRemaining(notChosen, true);
            }
        }

        public override void Resolve()
        {
            Choice.Resolve();
        }
    }
}