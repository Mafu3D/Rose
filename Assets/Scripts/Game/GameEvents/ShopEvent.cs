using System;
using System.Collections.Generic;
using Project.Items;
using UnityEngine;

namespace Project.Core.GameEvents
{
    public class ShopEvent : ChoiceEvent<ItemData>
    {
        // This should be mostly split into a special shop class and the event should just handle event stuff!
        private float priceModifier;
        private bool replaceOnBuy;
        private bool refreshable;
        private int refreshCost;

        private bool debug = true;

        public ShopEvent(int amount, float priceModifier, bool replaceOnBuy = false, bool refreshable = true, int refreshCost = 0) : base(amount, true)
        {
            this.priceModifier = priceModifier;
            this.replaceOnBuy = replaceOnBuy;
            this.refreshable = refreshable;
            this.refreshCost = refreshCost;
        }

        public event Action OnBuyEvent;
        public event Action OnCannotBuyEvent;
        public event Action OnRefreshEvent;

        public override void ChooseItem(int index)
        {
            if (Choice.GetAllItems()[index] == null)
            {
                if (debug)
                {
                    Debug.Log("!!! ITEM IS SOLD OUT !!!");
                    DebugShop();
                }
                OnCannotBuyEvent?.Invoke();
                return;
            }

            int price = (int)Mathf.Ceil(Choice.GetAllItems()[index].GoldValue * priceModifier);
            if (GameManager.Instance.Player.GoldTracker.Gold >= price)
            {
                Choice.ChooseItem(index);
                GameManager.Instance.Player.GoldTracker.RemoveGold(price);
                if (replaceOnBuy)
                {
                    Choice.TEMPReplaceLikeArrayForShop(GameManager.Instance.ItemDeck.Draw(), index);
                }
                else
                {
                    Choice.TEMPReplaceLikeArrayForShop(null, index);
                }
                Choice.Resolve();
                OnBuyEvent?.Invoke();

                if (debug)
                {
                    Debug.Log("!!! Bought item !!!");
                }
            }
            else
            {
                OnCannotBuyEvent?.Invoke();
                if (debug)
                {
                    Debug.Log("!!! NOT ENOUGH GOLD !!!");
                }
            }

            if (debug)
            {
                DebugShop();
            }
        }

        public override void GenerateChoices()
        {
            List<ItemData> choices = GameManager.Instance.ItemDeck.DrawMultiple(amount);
            if (choices.Count == 0) return;
            Choice = new Choice<ItemData>(choices, ResolveCallback);

            // Debug
            if (debug)
            {
                Debug.Log("!!! SHOP IS OPEN !!!");
                DebugShop();
            }
        }

        public override void Resolve()
        {
            GameManager.Instance.ItemDeck.AddToRemaining(Choice.GetNotChosen(), true);
        }

        protected override void ResolveCallback(List<ItemData> chosen, List<ItemData> notChosen)
        {
            foreach (ItemData itemData in chosen)
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
            Choice.Reset();
        }

        public void Refresh()
        {
            if (refreshable)
            {
                if (GameManager.Instance.Player.GoldTracker.Gold >= refreshCost)
                {
                    List<ItemData> oldChoice = Choice.GetNotChosen();
                    GenerateChoices();
                    GameManager.Instance.ItemDeck.AddToRemaining(oldChoice, true);

                    GameManager.Instance.Player.GoldTracker.RemoveGold(refreshCost);
                    OnRefreshEvent?.Invoke();
                    if (debug)
                    {
                        Debug.Log("!!! REFRESHED !!!");
                    }
                }
                else
                {
                    if (debug)
                    {
                        Debug.Log("!!! NOT GOLD ENOUGH TO REFRESH !!!");
                    }
                }
                if (debug)
                {
                   DebugShop();
                }
            }
        }

        private void DebugShop()
        {
            //Debug
            Debug.Log("---------------------------------------------");
            List<ItemData> itemDatas = Choice.GetAllItems();
            for (int i = 0; i < itemDatas.Count; i++)
            {
                if (itemDatas[i] == null) Debug.Log($"({i + 1}) SOLD OUT");
                else
                {
                    Debug.Log($"({i + 1}) {itemDatas[i].Name} - {itemDatas[i].Description} :: {(int)Mathf.Ceil(itemDatas[i].GoldValue * priceModifier)}g");
                }
            }

            if (refreshable)
            {
                Debug.Log($"(9) - Refresh the shop! : {refreshCost}");
            }
            Debug.Log("---------------------------------------------");
        }
    }
}