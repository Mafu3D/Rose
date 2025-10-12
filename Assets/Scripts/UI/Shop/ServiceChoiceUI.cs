using Project.GameplayEffects;
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
        GameplayEffectStrategy effect;

        internal void SetShopSlotNumber(int num) => shopNumberTMPText.text = num.ToString();

        internal void DisplayItemData(GameplayEffectStrategy effect, string name, int cost)
        {
            this.effect = effect;
            nameTMPText.text = name;
            costContainer.SetActive(true);
            costTMPText.text = cost.ToString();
            // image.sprite = sprite;
        }

        internal void DisplaySoldOut()
        {
            costContainer.SetActive(false);
            nameTMPText.text = "SOLD OUT";
            image.sprite = soldOutSprite;
        }

        public GameplayEffectStrategy GetEffect() => effect;
    }
}
