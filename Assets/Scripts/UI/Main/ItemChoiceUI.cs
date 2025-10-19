using System;
using System.Collections.Generic;
using Project.Core.GameEvents;
using Project.GameTiles;
using Project.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.MainUI
{
    public class ItemChoiceUI : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private GameObject mainContainer;
        [SerializeField] private GameObject displayPrefab;
        [SerializeField] private List<RectTransform> displayParentTransforms;
        List<GameObject> displayedObjects = new();
        [SerializeField] private Button exitButton;

        void Awake()
        {
            gameManager.OnGameStartEvent += Initialize;
        }

        void Initialize()
        {
            gameManager.GameEventManager.OnItemDrawStarted += DisplayObjects;
            gameManager.GameEventManager.OnItemDrawEnded += DestroyedDisplayedObjects;
        }

        private void DisplayObjects(IGameEvent gameEvent)
        {
            ItemChoiceEvent itemChoiceEvent = gameEvent as ItemChoiceEvent;
            List<ItemData> items = itemChoiceEvent.Choice.GetAllItems();

            for (int i = 0; i < items.Count; i++)
            {
                PopulateDisplay(items[i], i+1, displayParentTransforms[i], gameEvent);
            }
            mainContainer.SetActive(true);

            exitButton.onClick.AddListener(Exit);
        }

        private void Exit()
        {
            ChoiceEvent<ItemData> choiceEvent = GameManager.Instance.GameEventManager.CurrentItemChoiceEvent;
            if (choiceEvent.IsExitable)
            {
                choiceEvent.Resolve();
                GameManager.Instance.GameEventManager.EndItemDrawEvent();
            }
        }

        private void PopulateDisplay(ItemData item, int number, Transform parent, IGameEvent gameEvent)
        {
            GameObject displayedObeject = Instantiate(displayPrefab, parent.position, Quaternion.identity, parent);
            ItemChoiceDisplay display = displayedObeject.GetComponent<ItemChoiceDisplay>();
            display.RegisterDisplayItem(item, number, gameEvent);
            displayedObjects.Add(displayedObeject);
        }

        private void DestroyedDisplayedObjects(IGameEvent _)
        {
            foreach (GameObject displayedCard in displayedObjects)
            {
                Destroy(displayedCard);
            }

            mainContainer.SetActive(false);
        }
    }
}