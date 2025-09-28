using Project.GameNode;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.BattleUI
{
    public class AttributesUI : MonoBehaviour
    {
        [SerializeField] Node combatant;
        [SerializeField] TMP_Text healthText;
        [SerializeField] TMP_Text maxHealthText;
        [SerializeField] TMP_Text armorText;
        [SerializeField] TMP_Text speedText;
        [SerializeField] TMP_Text strengthText;
        [SerializeField] TMP_Text magicText;
        [SerializeField] TMP_Text dexterityText;

        public void Update() {
            healthText.text = combatant.Attributes.GetAttributeValue(Attributes.AttributeType.Health).ToString();
            maxHealthText.text = combatant.Attributes.GetMaxAttributeValue(Attributes.AttributeType.Health).ToString();
            armorText.text = combatant.Attributes.GetAttributeValue(Attributes.AttributeType.Armor).ToString();
            speedText.text = combatant.Attributes.GetAttributeValue(Attributes.AttributeType.Speed).ToString();
            strengthText.text = combatant.Attributes.GetAttributeValue(Attributes.AttributeType.Strength).ToString();
            magicText.text = combatant.Attributes.GetAttributeValue(Attributes.AttributeType.Magic).ToString();
            dexterityText.text = combatant.Attributes.GetAttributeValue(Attributes.AttributeType.Dexterity).ToString();
        }
    }
}