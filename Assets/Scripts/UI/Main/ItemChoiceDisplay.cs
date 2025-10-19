using Project.Core.GameEvents;
using Project.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.MainUI
{
    public class ItemChoiceDisplay : MonoBehaviour
    {
        [SerializeField] TMP_Text displayName;
        [SerializeField] TMP_Text descriptionText;
        [SerializeField] TMP_Text choiceNumberText;
        [SerializeField] Image image;
        [SerializeField] TMP_Text typeText;
        [SerializeField] GameObject commonIcon;
        [SerializeField] GameObject uncommonIcon;
        [SerializeField] GameObject rareIcon;
        [SerializeField] GameObject legendaryIcon;
        [SerializeField] Button button;

        [SerializeField] GameObject attributesDisplayContainer;

        int choiceNumber;
        IGameEvent gameEvent;

        public void RegisterDisplayItem(ItemData itemData, int choiceNumber, IGameEvent gameEvent)
        {
            this.choiceNumber = choiceNumber;
            this.gameEvent = gameEvent;

            displayName.text = itemData.Name;
            descriptionText.text = itemData.Description;
            image.sprite = itemData.Sprite;
            choiceNumberText.text = choiceNumber.ToString();

            switch (itemData.ItemType)
            {
                case ItemType.Basic:
                    typeText.text = "Held";
                    break;
                case ItemType.Weapon:
                    typeText.text = "Weapon";
                    break;
                case ItemType.Offhand:
                    typeText.text = "Offhand";
                    break;
            }

            switch (itemData.Rarity)
            {
                case ItemRarity.Common:
                    commonIcon.SetActive(true);
                    uncommonIcon.SetActive(false);
                    rareIcon.SetActive(false);
                    legendaryIcon.SetActive(false);
                    break;
                case ItemRarity.Uncommon:
                    commonIcon.SetActive(false);
                    uncommonIcon.SetActive(true);
                    rareIcon.SetActive(false);
                    legendaryIcon.SetActive(false);
                    break;
                case ItemRarity.Rare:
                    commonIcon.SetActive(false);
                    uncommonIcon.SetActive(false);
                    rareIcon.SetActive(true);
                    legendaryIcon.SetActive(false);
                    break;
                case ItemRarity.Legendary:
                    commonIcon.SetActive(false);
                    uncommonIcon.SetActive(false);
                    rareIcon.SetActive(false);
                    legendaryIcon.SetActive(true);
                    break;
            }


            button.onClick.AddListener(Choose);
        }

        private void Choose()
        {
            Debug.Log("button clicked! " + choiceNumber);
            ItemChoiceEvent itemChoiceEvent = gameEvent as ItemChoiceEvent;
            itemChoiceEvent.ChooseItem(choiceNumber - 1);
            itemChoiceEvent.Resolve();
            GameManager.Instance.GameEventManager.EndItemDrawEvent();
        }
    }
}