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

        private void Start()
        {
            combatant.Attributes.GetAttribute(Attributes.AttributeType.Health).OnValueChanged += UpdateHealth;
            combatant.Attributes.GetAttribute(Attributes.AttributeType.Armor).OnValueChanged += UpdateArmor;
            combatant.Attributes.GetAttribute(Attributes.AttributeType.Speed).OnValueChanged += UpdateSpeed;
            combatant.Attributes.GetAttribute(Attributes.AttributeType.Strength).OnValueChanged += UpdateStrength;
            combatant.Attributes.GetAttribute(Attributes.AttributeType.Magic).OnValueChanged += UpdateMagic;
            combatant.Attributes.GetAttribute(Attributes.AttributeType.Dexterity).OnValueChanged += UpdateDexterity;

            UpdateHealth();
            UpdateArmor();
            UpdateSpeed();
            UpdateStrength();
            UpdateMagic();
            UpdateDexterity();
        }

        void OnDisable()
        {
            combatant.Attributes.GetAttribute(Attributes.AttributeType.Health).OnValueChanged -= UpdateHealth;
            combatant.Attributes.GetAttribute(Attributes.AttributeType.Armor).OnValueChanged -= UpdateArmor;
            combatant.Attributes.GetAttribute(Attributes.AttributeType.Speed).OnValueChanged -= UpdateSpeed;
            combatant.Attributes.GetAttribute(Attributes.AttributeType.Strength).OnValueChanged -= UpdateStrength;
            combatant.Attributes.GetAttribute(Attributes.AttributeType.Magic).OnValueChanged -= UpdateMagic;
            combatant.Attributes.GetAttribute(Attributes.AttributeType.Dexterity).OnValueChanged -= UpdateDexterity;
        }

        private void UpdateHealth()
        {
            healthText.text = combatant.Attributes.GetAttributeValue(Attributes.AttributeType.Health).ToString();
            maxHealthText.text = combatant.Attributes.GetMaxAttributeValue(Attributes.AttributeType.Health).ToString();

        }
        private void UpdateArmor()
        {
            armorText.text = combatant.Attributes.GetAttributeValue(Attributes.AttributeType.Armor).ToString();
        }
        private void UpdateSpeed()
        {
            speedText.text = combatant.Attributes.GetAttributeValue(Attributes.AttributeType.Speed).ToString();
        }
        private void UpdateStrength()
        {
            strengthText.text = combatant.Attributes.GetAttributeValue(Attributes.AttributeType.Strength).ToString();
        }
        private void UpdateMagic()
        {
            magicText.text = combatant.Attributes.GetAttributeValue(Attributes.AttributeType.Magic).ToString();
        }
        private void UpdateDexterity()
        {
            dexterityText.text = combatant.Attributes.GetAttributeValue(Attributes.AttributeType.Dexterity).ToString();
        }
    }
}