using Project.Decks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.MainUI
{
    public class CardChoiceDisplay : MonoBehaviour
    {
        [SerializeField] TMP_Text displayName;
        [SerializeField] TMP_Text descriptionText;
        [SerializeField] TMP_Text numberText;
        [SerializeField] Image image;

        public void DisplayCard(Card card, int number)
        {
            displayName.text = card.Name;
            descriptionText.text = card.DisplayText;
            image.sprite = card.Sprite;
            numberText.text = number.ToString();
        }
    }
}