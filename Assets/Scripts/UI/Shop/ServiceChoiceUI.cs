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
        ServiceDefinition serviceDefinition;

        internal void SetShopSlotNumber(int num) => shopNumberTMPText.text = num.ToString();

        internal void DisplayItemData(ServiceDefinition serviceDefinition)
        {
            this.serviceDefinition = serviceDefinition;
            nameTMPText.text = serviceDefinition.DisplayName;
            costContainer.SetActive(true);
            costTMPText.text = serviceDefinition.Cost.ToString();
            image.sprite = serviceDefinition.DisplaySprite;
        }

        internal void DisplaySoldOut()
        {
            costContainer.SetActive(false);
            nameTMPText.text = "SOLD OUT";
            image.sprite = soldOutSprite;
        }

        public ServiceDefinition GetServiceDefinition() => serviceDefinition;
    }
}
