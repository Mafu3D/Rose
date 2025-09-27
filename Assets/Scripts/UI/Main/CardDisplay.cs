using Project.Decks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.MainUI
{
    public class CardDisplay : MonoBehaviour
    {
        [SerializeField] TMP_Text displayName;
        [SerializeField] TMP_Text descriptionText;
        [SerializeField] Image image;

        public void DisplayCard(Card card)
        {
            displayName.text = card.Name;
            descriptionText.text = card.DisplayText;
            image.sprite = card.Sprite;
        }
    }
}