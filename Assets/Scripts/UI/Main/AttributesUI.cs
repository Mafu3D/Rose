using Project.GameTiles;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.BattleUI
{
    public class AttributesUI : MonoBehaviour
    {
        [SerializeField] Tile playerCharacter;
        [SerializeField] TMP_Text healthText;
        [SerializeField] TMP_Text maxHealthText;
        [SerializeField] TMP_Text armorText;
        [SerializeField] TMP_Text speedText;
        [SerializeField] TMP_Text strengthText;
        [SerializeField] TMP_Text magicText;
        [SerializeField] TMP_Text dexterityText;

        private void Start()
        {
            playerCharacter.Character.Attributes.GetAttribute(Attributes.AttributeType.Health).OnValueChanged += UpdateHealth;
            playerCharacter.Character.Attributes.GetAttribute(Attributes.AttributeType.Armor).OnValueChanged += UpdateArmor;
            playerCharacter.Character.Attributes.GetAttribute(Attributes.AttributeType.Speed).OnValueChanged += UpdateSpeed;
            playerCharacter.Character.Attributes.GetAttribute(Attributes.AttributeType.Strength).OnValueChanged += UpdateStrength;
            playerCharacter.Character.Attributes.GetAttribute(Attributes.AttributeType.Magic).OnValueChanged += UpdateMagic;
            playerCharacter.Character.Attributes.GetAttribute(Attributes.AttributeType.Dexterity).OnValueChanged += UpdateDexterity;

            UpdateHealth();
            UpdateArmor();
            UpdateSpeed();
            UpdateStrength();
            UpdateMagic();
            UpdateDexterity();
        }

        void OnDestroy()
        {
            playerCharacter.Character.Attributes.GetAttribute(Attributes.AttributeType.Health).OnValueChanged -= UpdateHealth;
            playerCharacter.Character.Attributes.GetAttribute(Attributes.AttributeType.Armor).OnValueChanged -= UpdateArmor;
            playerCharacter.Character.Attributes.GetAttribute(Attributes.AttributeType.Speed).OnValueChanged -= UpdateSpeed;
            playerCharacter.Character.Attributes.GetAttribute(Attributes.AttributeType.Strength).OnValueChanged -= UpdateStrength;
            playerCharacter.Character.Attributes.GetAttribute(Attributes.AttributeType.Magic).OnValueChanged -= UpdateMagic;
            playerCharacter.Character.Attributes.GetAttribute(Attributes.AttributeType.Dexterity).OnValueChanged -= UpdateDexterity;
        }

        private void UpdateHealth()
        {
            healthText.text = playerCharacter.Character.Attributes.GetAttributeValue(Attributes.AttributeType.Health).ToString();
            maxHealthText.text = playerCharacter.Character.Attributes.GetMaxAttributeValue(Attributes.AttributeType.Health).ToString();

        }
        private void UpdateArmor()
        {
            armorText.text = playerCharacter.Character.Attributes.GetAttributeValue(Attributes.AttributeType.Armor).ToString();
        }
        private void UpdateSpeed()
        {
            speedText.text = playerCharacter.Character.Attributes.GetAttributeValue(Attributes.AttributeType.Speed).ToString();
        }
        private void UpdateStrength()
        {
            strengthText.text = playerCharacter.Character.Attributes.GetAttributeValue(Attributes.AttributeType.Strength).ToString();
        }
        private void UpdateMagic()
        {
            magicText.text = playerCharacter.Character.Attributes.GetAttributeValue(Attributes.AttributeType.Magic).ToString();
        }
        private void UpdateDexterity()
        {
            dexterityText.text = playerCharacter.Character.Attributes.GetAttributeValue(Attributes.AttributeType.Dexterity).ToString();
        }
    }
}