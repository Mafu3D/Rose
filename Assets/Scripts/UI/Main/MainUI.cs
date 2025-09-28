using Project;
using Project.Decks;
using Project.Items;
using UnityEngine;

namespace Project.UI.MainUI
{
    public class MainUI : Singleton<MainUI>
    {
        [SerializeField] private GameObject cardDisplayPrefab;
        [SerializeField] private RectTransform cardDisplayTransform;

        [SerializeField] private GameObject treasureChoicePrefab;
        [SerializeField] private RectTransform treasureChoiceTransform;

        GameObject currentlyDisplayedCard;
        GameObject currentTreasureChoice;

        public void DisplayCard(Card card)
        {
            currentlyDisplayedCard = Instantiate(cardDisplayPrefab, cardDisplayTransform.position, Quaternion.identity, cardDisplayTransform);
            CardDisplay cardDisplay = currentlyDisplayedCard.GetComponent<CardDisplay>();
            cardDisplay.DisplayCard(card);
        }

        public void DestroyDisplayedCard()
        {
            if (currentlyDisplayedCard != null)
            {
                Destroy(currentlyDisplayedCard);
            }
        }

        public void DisplayTreasureChoice(TreasureChoice treasureChoice)
        {
            currentTreasureChoice = Instantiate(treasureChoicePrefab, treasureChoiceTransform.position, Quaternion.identity, treasureChoiceTransform);
            TreasureChoiceDisplay treasureChoiceDisplay = currentTreasureChoice.GetComponent<TreasureChoiceDisplay>();
            treasureChoiceDisplay.DisplayChoices(treasureChoice);
        }

        public void DestroyTreasureChoice()
        {
            if (currentTreasureChoice != null)
            {
                Destroy(currentTreasureChoice);
            }
        }
    }
}