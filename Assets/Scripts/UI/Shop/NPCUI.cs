using System.Collections.Generic;
using Project.Core.GameEvents;
using Project.Custom;
using Project.GameplayEffects;
using Project.Items;
using TMPro;
using UnityEngine;

namespace Project.UI.Shop
{
    public class NPCUI : MonoBehaviour
    {
        [Header("Game Manager")]
        [SerializeField] private GameManager gameManager;

        [Header("Main")]
        [SerializeField] private GameObject mainContainer;

        [Header("Item Grid")]
        [SerializeField] RectTransform gridContainer;
        [SerializeField] GameObject serviceChoiceUIPrefab;

        List<GameObject> serviceChoiceObjects = new();

        ServiceEvent serviceEvent;

        void Awake()
        {
            gameManager.OnGameStartEvent += Initialize;
        }

        void Initialize()
        {
            gameManager.GameEventManager.OnNPCServiceStarted += ShowUI;
            gameManager.GameEventManager.OnNPCServiceEnded += HideUI;
        }

        private void ShowUI(IGameEvent gameEvent)
        {
            mainContainer.SetActive(true);
            serviceEvent = gameEvent as ServiceEvent;
            Populate();

            serviceEvent.OnServicesUpdated += Populate;
        }

        private void Populate()
        {
            PopulateGrid();
        }


        private void PopulateGrid()
        {
            // TEMP
            foreach (GameObject gameObject in serviceChoiceObjects)
            {
                Destroy(gameObject);
            }

            List<SerializableKeyValuePair<GameplayEffectStrategy, int>> services = serviceEvent.Choice.GetAllItems();
            for (int i = 0; i < services.Count; i++)
            {
                GameObject shopItemUIGameObject;
                shopItemUIGameObject = Instantiate(serviceChoiceUIPrefab, gridContainer);
                serviceChoiceObjects.Add(shopItemUIGameObject);
                ServiceChoiceUI shopItemUI = shopItemUIGameObject.GetComponent<ServiceChoiceUI>();
                shopItemUI.SetShopSlotNumber(i + 1);
                if (services[i] != null)
                {
                    shopItemUI.DisplayItemData(services[i].Key, "Service", services[i].Value);
                }
                else
                {
                    shopItemUI.DisplaySoldOut();
                }
            }
        }

        private void HideUI(IGameEvent gameEvent)
        {
            mainContainer.SetActive(false);
            serviceEvent.OnServicesUpdated -= Populate;
        }
    }
}
