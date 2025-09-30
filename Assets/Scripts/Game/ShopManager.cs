using System;
using Project.Items;

namespace Project.Core
{
    public class ShopManager
    {
        public event Action OnNewShopEvent;
        public event Action OnConcludeShopEvent;

        public bool ShopChoiceIsActive => ActiveShopChoice != null;
        public Choice<Item> ActiveShopChoice;

        public void OpenNewShop(int amount)
        {

        }
    }

    public class Shop
    {
        int amount;

        public Shop(int amount)
        {
            this.amount = amount;
        }

        public void RefreshShop()
        {

        }

        public void BuyItem(int index)
        {

        }

        public void TakeItem(int index)
        {

        }
    }
}