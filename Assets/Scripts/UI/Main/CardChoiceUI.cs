using System.Collections.Generic;
using Project.Decks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.MainUI
{
    public class CardChoiceUI : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private GameObject cardDisplayPrefab;
        [SerializeField] private RectTransform centerCardDisplayTransform;
        [SerializeField] private RectTransform leftCardDisplayTransform;
        [SerializeField] private RectTransform rightCardDisplayTransform;

        List<GameObject> displayedCards = new();

        void Awake()
        {
            gameManager.OnGameStartEvent += Initialize;
        }

        void Initialize()
        {
            gameManager.CardDrawManager.OnNewCardDrawEvent += DisplayCards;
            gameManager.CardDrawManager.OnConcludeCardDrawEvent += DestroyDisplayedCards;
        }

        private void DisplayCards()
        {
            // if (!gameManager.CardDrawManager.CardChoiceIsActive) { return; }

            // Choice<Card> cardChoice = gameManager.CardDrawManager.ActiveCardChoice;
            // List<Card> cards = cardChoice.items;

            // // Single card
            // if (cards.Count == 1)
            // {
            //     GameObject centerDisplayedCard = Instantiate(cardDisplayPrefab, centerCardDisplayTransform.position, Quaternion.identity, centerCardDisplayTransform);
            //     CardDisplay centerCardDisplay = centerDisplayedCard.GetComponent<CardDisplay>();
            //     centerCardDisplay.DisplayCard(cards[0]);
            //     displayedCards.Add(centerDisplayedCard);
            // }

            // // Choice of two
            // else if (cards.Count == 2)
            // {
            //     GameObject displayedCardLeft = Instantiate(cardDisplayPrefab, leftCardDisplayTransform.position, Quaternion.identity, leftCardDisplayTransform);
            //     CardDisplay leftCardDisplay = displayedCardLeft.GetComponent<CardDisplay>();
            //     leftCardDisplay.DisplayCard(cards[0]);
            //     displayedCards.Add(displayedCardLeft);

            //     GameObject displayedCardRight = Instantiate(cardDisplayPrefab, rightCardDisplayTransform.position, Quaternion.identity, rightCardDisplayTransform);
            //     CardDisplay rightCardDisplay = displayedCardRight.GetComponent<CardDisplay>();
            //     rightCardDisplay.DisplayCard(cards[1]);
            //     displayedCards.Add(displayedCardRight);
            // }
        }

        private void DestroyDisplayedCards()
        {
            foreach (GameObject displayedCard in displayedCards)
            {
                Destroy(displayedCard);
            }
        }
    }
}