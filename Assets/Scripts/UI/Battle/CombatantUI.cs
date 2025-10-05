using System.Collections.Generic;
using Project.Combat;
using Project.Combat.StatusEffects;
using Project.GameTiles;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.BattleUI
{
    public class CombatantUI : MonoBehaviour
    {
        [SerializeField] Image image;
        [SerializeField] TMP_Text displayName;
        [SerializeField] TMP_Text description;
        [SerializeField] TMP_Text healthText;
        [SerializeField] TMP_Text armorText;
        [SerializeField] TMP_Text speedText;
        [SerializeField] TMP_Text strengthText;
        [SerializeField] TMP_Text magicText;
        [SerializeField] TMP_Text dexterityText;
        [SerializeField] TMP_Text statusEffectText;

        Character combatant;

        public void InitializeCombatant(Character combatant)
        {
            image.sprite = combatant.Sprite;
            displayName.text = combatant.DisplayName;
            this.description.text = combatant.Description;

            this.combatant = combatant;

            this.statusEffectText.text = "";

            UpdateStats();
        }

        public void UpdateStats()
        {
            healthText.text = combatant.Attributes.GetAttributeValue(Attributes.AttributeType.Health).ToString();
            armorText.text = combatant.Attributes.GetAttributeValue(Attributes.AttributeType.Armor).ToString();
            speedText.text = combatant.Attributes.GetAttributeValue(Attributes.AttributeType.Speed).ToString();
            strengthText.text = combatant.Attributes.GetAttributeValue(Attributes.AttributeType.Strength).ToString();
            magicText.text = combatant.Attributes.GetAttributeValue(Attributes.AttributeType.Magic).ToString();
            dexterityText.text = combatant.Attributes.GetAttributeValue(Attributes.AttributeType.Dexterity).ToString();
        }

        public void UpdateStatusEffects()
        {
            string text = "";
            foreach (StatusEffect statusEffect in combatant.StatusEffectManager.GetRegisteredStatusEffects().Values)
            {
                text += $"{statusEffect.DisplayName} {statusEffect.Stacks} \n";
            }
            statusEffectText.text = text;
        }
    }
}