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
        [SerializeField] TMP_Text choiceNumber;
        [SerializeField] Image image;

        [SerializeField] GameObject attributesDisplayContainer;

        public void DisplayItem(Item item, int choiceNumber)
        {
            displayName.text = item.ItemData.Name;
            descriptionText.text = item.ItemData.Description;
            image.sprite = item.ItemData.Sprite;
            this.choiceNumber.text = choiceNumber.ToString();

            // [SerializeField] public int HealthModifier = 0;
            // [SerializeField] public int ArmorModifier = 0;
            // [SerializeField] public int StrengthModifier = 0;
            // [SerializeField] public int MagicModifier = 0;
            // [SerializeField] public int DexterityModifier = 0;
            // [SerializeField] public int SpeedModifier = 0;
        }

    }
}