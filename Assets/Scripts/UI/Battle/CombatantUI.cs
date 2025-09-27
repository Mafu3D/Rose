using Project.GameNode;
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

        CombatNode combatant;

        public void InitializeCombatant(CombatNode combatantNode, Sprite sprite, string name, string description)
        {
            image.sprite = sprite;
            displayName.text = name;
            this.description.text = description;

            combatant = combatantNode;

            UpdateStats();
        }

        public void UpdateStats()
        {
            healthText.text = combatant.GetHealthValue().ToString();
            armorText.text = combatant.GetArmorValue().ToString();
            speedText.text = combatant.GetSpeedValue().ToString();
            strengthText.text = combatant.GetStrengthValue().ToString();
            magicText.text = combatant.GetMagicValue().ToString();
            dexterityText.text = combatant.GetDexterityValue().ToString();
        }
    }
}