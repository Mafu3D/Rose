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
        ItemData itemData;

        internal void SetShopSlotNumber(int num) => shopNumberTMPText.text = num.ToString();

        internal void DisplayItemData(ItemData itemData)
        {
            this.itemData = itemData;
            nameTMPText.text = itemData.Name;
            costContainer.SetActive(true);

            int goldValue = GetGoldValue(itemData.Rarity);
            int price = (int)Mathf.Ceil(goldValue); // does not factor in price modifier!
            costTMPText.text = price.ToString();
            image.sprite = itemData.Sprite;
        }

        internal void DisplaySoldOut()
        {
            costContainer.SetActive(false);
            nameTMPText.text = "SOLD OUT";
            image.sprite = soldOutSprite;
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
    }
}
