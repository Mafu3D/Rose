using Project.Core.GameEvents;
using Project.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Shop
{
    public class ShopItemUI : MonoBehaviour
    {
        [SerializeField] TMP_Text nameTMPText;
        [SerializeField] GameObject costContainer;
        [SerializeField] TMP_Text costTMPText;
        [SerializeField] TMP_Text shopNumberTMPText;
        [SerializeField] Image image;
        [SerializeField] Sprite soldOutSprite;
        [SerializeField] Button button;
        ItemData itemData;
        int choiceNumber;

        internal void SetShopSlotNumber(int num)
        {
            shopNumberTMPText.text = num.ToString();
            choiceNumber = num;
        }


        internal void DisplayItemData(ItemData itemData)
        {
            this.itemData = itemData;
            nameTMPText.text = itemData.Name;
            costContainer.SetActive(true);

            int goldValue = GetGoldValue(itemData.Rarity);
            int price = (int)Mathf.Ceil(goldValue); // does not factor in price modifier!
            costTMPText.text = price.ToString();
            image.sprite = itemData.Sprite;

            // button.onClick.RemoveAllListeners();
            // button.onClick.RemoveListener(Choose);
            // button.onClick.AddListener(Choose);
        }

        void OnDestroy()
        {
            // button.onClick.RemoveAllListeners();
        }

        internal void DisplaySoldOut()
        {
            costContainer.SetActive(false);
            nameTMPText.text = "SOLD OUT";
            image.sprite = soldOutSprite;
            // button.onClick.RemoveAllListeners();
            // button.onClick.RemoveListener(Choose);
        }

        public ItemData GetItemData() => itemData;

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

        public void Choose()
        {
            button.onClick.RemoveAllListeners();
            if (GameManager.Instance.Hero.Character.Inventory.InventoryIsFull)
            {
                CalloutUI.Instance.QueueCallout("Inventory full! Please discard an item by left clicking it.");
                return;
            }

            ShopEvent shopEvent = GameManager.Instance.GameEventManager.CurrentShopEvent;
            shopEvent.ChooseItem(choiceNumber - 1);
            shopEvent.Resolve();
        }
    }
}
