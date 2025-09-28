using System.Collections.Generic;
using System.Linq;
using Project;
using Project.Decks;
using Project.Items;
using UnityEngine;

namespace Project.UI.MainUI
{
    public class MainUI : Singleton<MainUI>
    {
        [SerializeField] private GameObject cardDisplayPrefab;
        [SerializeField] private RectTransform centerCardDisplayTransform;
        [SerializeField] private RectTransform leftCardDisplayTransform;
        [SerializeField] private RectTransform rightCardDisplayTransform;

        [SerializeField] private GameObject treasureChoicePrefab;
        [SerializeField] private RectTransform treasureChoiceTransform;

        List<GameObject> displayedCards = new();
        GameObject currentTreasureChoice;

        public void DisplayCards(List<Card> cards)
        {
            // Single card
            if (cards.Count == 1)
            {
                GameObject centerDisplayedCard = Instantiate(cardDisplayPrefab, centerCardDisplayTransform.position, Quaternion.identity, centerCardDisplayTransform);
                CardDisplay centerCardDisplay = centerDisplayedCard.GetComponent<CardDisplay>();
                centerCardDisplay.DisplayCard(cards[0]);
                displayedCards.Add(centerDisplayedCard);
            }

            // Choice of two
            else if (cards.Count == 2)
            {
                GameObject displayedCardLeft = Instantiate(cardDisplayPrefab, leftCardDisplayTransform.position, Quaternion.identity, leftCardDisplayTransform);
                CardDisplay leftCardDisplay = displayedCardLeft.GetComponent<CardDisplay>();
                leftCardDisplay.DisplayCard(cards[0]);
                displayedCards.Add(displayedCardLeft);

                GameObject displayedCardRight = Instantiate(cardDisplayPrefab, rightCardDisplayTransform.position, Quaternion.identity, rightCardDisplayTransform);
                CardDisplay rightCardDisplay = displayedCardRight.GetComponent<CardDisplay>();
                rightCardDisplay.DisplayCard(cards[1]);
                displayedCards.Add(displayedCardRight);
            }
        }

        public void DestroyDisplayedCards()
        {
            foreach (GameObject displayedCard in displayedCards)
            {
                Destroy(displayedCard);
            }
        }

        public void DisplayTreasureChoice(Choice<Item> treasureChoice)
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