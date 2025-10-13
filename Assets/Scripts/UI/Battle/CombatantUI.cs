using System.Collections;
using EasyTextEffects;
using Project.Attributes;
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
        [SerializeField] GameObject activeCombatantContainer;
        [SerializeField] GameObject attackVFX;
        [SerializeField] Transform effectVFXLocator;

        Character combatant;

        void Awake()
        {
            activeCombatantContainer.SetActive(false);
            attackVFX.SetActive(false);
        }

        public void InitializeCombatant(Character combatant)
        {
            image.sprite = combatant.Sprite;
            displayName.text = combatant.DisplayName;
            this.description.text = combatant.CombatDescription;

            this.combatant = combatant;

            this.statusEffectText.text = "";

            UpdateStats();
        }

        public void UpdateStats()
        {
            UpdateStatText(healthText, AttributeType.Health);
            UpdateStatText(armorText, AttributeType.Armor);
            UpdateStatText(speedText, AttributeType.Speed);
            UpdateStatText(strengthText, AttributeType.Strength);
            UpdateStatText(magicText, AttributeType.Magic);
            UpdateStatText(dexterityText, AttributeType.Dexterity);
        }

        private void UpdateStatText(TMP_Text text, AttributeType attributeType)
        {
            int newValue = combatant.Attributes.GetAttributeValue(attributeType);
            int oldValue = int.Parse(text.text);
            if (newValue == oldValue) return;

            text.text = newValue.ToString();

            TextEffect textEffect = text.GetComponent<TextEffect>();
            if (textEffect != null)
            {
                if (newValue < oldValue)
                {
                    StartCoroutine(TextEffectRoutine(textEffect, "damage"));
                }
                else if (newValue > oldValue)
                {
                    StartCoroutine(TextEffectRoutine(textEffect, "heal"));
                }
            }
        }

        private IEnumerator TextEffectRoutine(TextEffect textEffect, string tag)
        {
            textEffect.StartManualEffect(tag);
            textEffect.StartManualEffect("changed");
            yield return new WaitForSecondsRealtime(0.4f);
            textEffect.StopManualEffects();
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

        public void PlayAttackVFX()
        {
            Animator animator = attackVFX.GetComponent<Animator>();
            if (animator != null)
            {
                StartCoroutine(PlayAttackVFXRoutine(animator));
            }
        }

        private IEnumerator PlayAttackVFXRoutine(Animator animator) {
            int speedHash = Animator.StringToHash("Speed");
            animator.SetFloat(speedHash, 1 / GameManager.Instance.AutoBattleSpeed);
            attackVFX.SetActive(true);
            int vfxAnimationHash = Animator.StringToHash("IdleVFX");
            animator.Play(vfxAnimationHash);

            yield return new WaitForSecondsRealtime(0.75f * GameManager.Instance.AutoBattleSpeed);
            attackVFX.SetActive(false);
        }

        public void PlayEffectVFX(GameObject VFXPrefab)
        {
            GameObject vfxObject = Instantiate(VFXPrefab, effectVFXLocator);
            StartCoroutine(PlayEffectVFXRoutine(vfxObject));
        }

        private IEnumerator PlayEffectVFXRoutine(GameObject vfxObject)
        {
            Animator animator = vfxObject.GetComponent<Animator>();
            int speedHash = Animator.StringToHash("Speed");
            animator.SetFloat(speedHash, 1 / GameManager.Instance.AutoBattleSpeed);
            if (animator != null)
            {
                int vfxAnimationHash = Animator.StringToHash("IdleVFX");
                animator.Play(vfxAnimationHash);

                yield return new WaitForSecondsRealtime(0.75f * GameManager.Instance.AutoBattleSpeed);
                Destroy(vfxObject);
            }
        }

        public void SetActiveCombatant(bool value)
        {
            activeCombatantContainer.SetActive(value);
        }
    }
}