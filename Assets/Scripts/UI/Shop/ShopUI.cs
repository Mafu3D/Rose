using System;
using System.Collections.Generic;
using Project.Core.GameEvents;
using Project.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Shop
{
    public class ShopUI : MonoBehaviour
    {
        [Header("Game Manager")]
        [SerializeField] private GameManager gameManager;

        [Header("Main")]
        [SerializeField] private GameObject mainContainer;

        [Header("Item Grid")]
        [SerializeField] RectTransform gridContainer;
        [SerializeField] GameObject shopItemUIPrefab;

        [Header("Refresh")]
        [SerializeField] GameObject refreshContainer;
        [SerializeField] TMP_Text refreshTMPText;

        [SerializeField] List<ShopItemUI> shopItemUIs;

        List<GameObject> shopItemObjects = new();

        ShopEvent shopEvent;

        void Awake()
        {
            gameManager.OnGameStartEvent += Initialize;
        }

        void Initialize()
        {
            gameManager.GameEventManager.OnShopStarted += ShowUI;
            gameManager.GameEventManager.OnShopEnded += HideUI;
        }

        private void ShowUI(IGameEvent gameEvent)
        {
            mainContainer.SetActive(true);
            shopEvent = gameEvent as ShopEvent;
            Populate();

            shopEvent.OnShopUpdated += Populate;
        }

        private void Populate()
        {
            PopulateGrid();
            PopulateRefresh();
            // populate sell (Not Implemented)
        }

        private void PopulateRefresh()
        {
            if (shopEvent.Refreshable)
            {
                refreshContainer.SetActive(true);
                refreshTMPText.text = shopEvent.RefreshCost.ToString();
            }
            else
            {
                refreshContainer.SetActive(false);
            }
        }

        private void PopulateGrid()
        {
            List<ItemData> items = shopEvent.Choice.GetAllItems();
            for (int i = 0; i < items.Count; i++)
            {
                // GameObject shopItemUIGameObject;
                // shopItemUIGameObject = Instantiate(shopItemUIPrefab, gridContainer);
                // shopItemObjects.Add(shopItemUIGameObject);
                // ShopItemUI shopItemUI = shopItemUIGameObject.GetComponent<ShopItemUI>();
                ShopItemUI shopItemUI = shopItemUIs[i];
                shopItemUI.SetShopSlotNumber(i + 1);
                if (items[i] != null)
                {
                    shopItemUI.DisplayItemData(items[i]);
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

            shopEvent.OnShopUpdated -= Populate;
        }

        public void Exit()
        {
            ShopEvent shopEvent = GameManager.Instance.GameEventManager.CurrentShopEvent;
            if (shopEvent.IsExitable)
            {
                shopEvent.Resolve();
                GameManager.Instance.GameEventManager.EndShopEvent();
            }
        }

        public void Refresh()
        {
            ShopEvent shopEvent = GameManager.Instance.GameEventManager.CurrentShopEvent;
            shopEvent.Refresh();
        }
    }
}
