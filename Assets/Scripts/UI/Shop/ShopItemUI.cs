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
            costTMPText.text = itemData.GoldValue.ToString();
            image.sprite = itemData.Sprite;
        }

        internal void DisplaySoldOut()
        {
            costContainer.SetActive(false);
            nameTMPText.text = "SOLD OUT";
            image.sprite = soldOutSprite;
        }

        public ItemData GetItemData() => itemData;
    }
}
