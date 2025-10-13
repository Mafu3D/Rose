using System.Collections.Generic;
using Project.Items;
using UnityEngine;

namespace Project.Core.GameEvents
{
    public class InventoryChoiceEvent : ChoiceEvent<Item>
    {
        int chosenAmount;
        public InventoryChoiceEvent(int amount) : base(amount, true) { }

        public override void GenerateChoices()
        {
            List<Item> choices = GameManager.Instance.Player.HeroTile.Character.Inventory.GetAllItemsWithNulls();
            if (choices.Count == 0) return;
            Choice = new Choice<Item>(choices, ResolveCallback);

            foreach(Item item in choices)
            {
                if (item != null)
                {
                    Debug.Log(item.ItemData.Name);
                }
                else
                {
                    Debug.Log("NULL");
                }
            }
        }

        public override void ChooseItem(int index)
        {
            if (Choice.GetAllItems()[index] == null) return;
            Choice.ChooseItem(index);
            chosenAmount++;
            if (chosenAmount >= amount)
            {
                Resolve();
            }
        }

        public override void Resolve()
        {
            Choice.Resolve();
            GameManager.Instance.GameEventManager.EndInventoryChoiceEvent();
        }

        protected override void ResolveCallback(List<Item> chosen, List<Item> notChosen)
        {
            // oh okay so all inventory choices have to result in an item being removed hmmmmm
            foreach(Item item in chosen)
            {
                GameManager.Instance.Player.HeroTile.Character.Inventory.RemoveItem(item);
            }
        }
    }
}