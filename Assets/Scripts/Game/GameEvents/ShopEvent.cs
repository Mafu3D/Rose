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
        public bool Refreshable { get; private set; }
        public int RefreshCost { get; private set; }
        List<ItemData> existingInventory = new();

        private bool debug = true;

        public ShopEvent(int amount, float priceModifier, bool replaceOnBuy = false, bool refreshable = true, int refreshCost = 0, List<ItemData> existingInventory = null) : base(amount, true)
        {
            this.priceModifier = priceModifier;
            this.replaceOnBuy = replaceOnBuy;
            this.Refreshable = refreshable;
            this.RefreshCost = refreshCost;
            this.existingInventory = existingInventory;
        }

        public event Action OnBuyEvent;
        public event Action OnCannotBuyEvent;
        public event Action OnRefreshEvent;
        public event Action OnShopUpdated;

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

            int goldValue = GetGoldValue(Choice.GetAllItems()[index].Rarity);
            int price = (int)Mathf.Ceil(goldValue * priceModifier);
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
                OnShopUpdated?.Invoke();

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

        private int GetGoldValue(ItemRarity rarity)
        {
            switch (rarity)
            {
                case ItemRarity.Common:
                    return 3;
                case ItemRarity.Uncommon:
                    return 5;
                case ItemRarity.Rare:
                    return 7;
                case ItemRarity.Legendary:
                    return 10;
            }
            return 3;
        }

        public override void GenerateChoices()
        {
            List<ItemData> choices = new();
            if (existingInventory != null && existingInventory.Count > 0)
            {
                choices = new List<ItemData>(existingInventory);
            }
            else
            {
                choices = GameManager.Instance.ItemDeck.DrawMultiple(amount);
            }
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
                        GameManager.Instance.Player.HeroTile.Character.Inventory.SwapEquippedWeapon(item);
                        break;
                    case ItemType.Offhand:
                        GameManager.Instance.Player.HeroTile.Character.Inventory.SwapEquippedOffhand(item);
                        break;
                    default:
                        GameManager.Instance.Player.HeroTile.Character.Inventory.AddItem(item);
                        break;
                }
            }
            Choice.Reset();
        }

        public void Refresh()
        {
            if (Refreshable)
            {
                if (GameManager.Instance.Player.GoldTracker.Gold >= RefreshCost)
                {
                    List<ItemData> oldChoice = Choice.GetNotChosen();
                    GenerateChoices();
                    GameManager.Instance.ItemDeck.AddToRemaining(oldChoice, true);

                    GameManager.Instance.Player.GoldTracker.RemoveGold(RefreshCost);
                    OnRefreshEvent?.Invoke();
                    OnShopUpdated?.Invoke();
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
                    int goldValue = GetGoldValue(Choice.GetAllItems()[i].Rarity);
                    Debug.Log($"({i + 1}) {itemDatas[i].Name} - {itemDatas[i].Description} :: {(int)Mathf.Ceil(goldValue * priceModifier)}g");
                }
            }

            if (Refreshable)
            {
                Debug.Log($"(9) - Refresh the shop! : {RefreshCost}");
            }
            Debug.Log("---------------------------------------------");
        }
    }
}