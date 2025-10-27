using System;
using Project.Core.GameEvents;
using Project.GameplayEffects;
using Project.NPCs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Shop
{
    public class ServiceChoiceUI : MonoBehaviour
    {
        [SerializeField] TMP_Text nameTMPText;
        [SerializeField] GameObject costContainer;
        [SerializeField] TMP_Text costTMPText;
        [SerializeField] TMP_Text shopNumberTMPText;
        [SerializeField] Image image;
        [SerializeField] Sprite soldOutSprite;
        [SerializeField] Button button;
        ServiceDefinition serviceDefinition;

        private int choiceNumber;

        internal void SetShopSlotNumber(int num)
        {
            choiceNumber = num;
            shopNumberTMPText.text = num.ToString();
        }

        internal void DisplayItemData(ServiceDefinition serviceDefinition)
        {
            this.serviceDefinition = serviceDefinition;
            nameTMPText.text = serviceDefinition.DisplayName;
            costContainer.SetActive(true);
            costTMPText.text = serviceDefinition.Cost.ToString();
            image.sprite = serviceDefinition.DisplaySprite;

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(Choose);
        }

        private void Choose()
        {
            button.onClick.RemoveAllListeners();
            ChoiceEvent<ServiceDefinition> choiceEvent = GameManager.Instance.GameEventManager.CurrentServiceEvent;
            choiceEvent.ChooseItem(choiceNumber - 1);
            choiceEvent.Resolve();
        }

        void OnDestroy()
        {
            button.onClick.RemoveAllListeners();
        }

        internal void DisplaySoldOut()
        {
            costContainer.SetActive(false);
            nameTMPText.text = "SOLD OUT";
            image.sprite = soldOutSprite;

            button.onClick.RemoveAllListeners();
        }

        public ServiceDefinition GetServiceDefinition() => serviceDefinition;
    }
}
