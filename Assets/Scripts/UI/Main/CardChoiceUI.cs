using System.Collections.Generic;
using Project.Core;
using Project.Core.GameEvents;
using Project.Decks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.MainUI
{
    public class CardChoiceUI : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private GameObject mainContainer;
        [SerializeField] private GameObject displayPrefab;
        [SerializeField] private List<RectTransform> displayParentTransforms;
        [SerializeField] private Button continueButton;

        List<GameObject> displayedObjects = new();

        void Awake()
        {
            gameManager.OnGameStartEvent += Initialize;
        }

        void Initialize()
        {
            gameManager.GameEventManager.OnCardDrawStarted += DisplayObjects;
            gameManager.GameEventManager.OnCardDrawEnded += DestroyedDisplayedObjects;
        }

        private void DisplayObjects(IGameEvent gameEvent)
        {
            CardChoiceEvent cardChoiceEvent = gameEvent as CardChoiceEvent;
            List<Card> cards = cardChoiceEvent.Choice.GetAllItems();

            for (int i = 0; i < cards.Count; i++)
            {
                PopulateDisplay(cards[i], i + 1, displayParentTransforms[i]);
            }
            mainContainer.SetActive(true);

            continueButton.gameObject.SetActive(true);
        }

        private void PopulateDisplay(Card card, int number, Transform parent)
        {
            GameObject displayedObject = Instantiate(displayPrefab, parent.position, Quaternion.identity, parent);
            CardChoiceDisplay display = displayedObject.GetComponent<CardChoiceDisplay>();
            display.DisplayCard(card, number);
            displayedObjects.Add(displayedObject);
        }

        private void DestroyedDisplayedObjects(IGameEvent _)
        {
            foreach (GameObject displayedCard in displayedObjects)
            {
                Destroy(displayedCard);
            }

            mainContainer.SetActive(false);
            continueButton.gameObject.SetActive(false);
        }

        public void OnContinueClicked()
        {
            CardChoiceEvent cardChoiceEvent = GameManager.Instance.GameEventManager.CurrentCardChoiceEvent;
            cardChoiceEvent.ChooseItem(0);
            cardChoiceEvent.Resolve();
            GameManager.Instance.GameEventManager.EndCardDrawEvent();
        }
    }
}