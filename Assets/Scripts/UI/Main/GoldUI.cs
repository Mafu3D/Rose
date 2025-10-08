using System;
using System.Collections;
using EasyTextEffects;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Project.UI.MainUI
{
    public class GoldUI : MonoBehaviour
    {
        [SerializeField] TMP_Text goldText;
        [SerializeField] GoldTracker goldTracker;

        void Start()
        {
            goldTracker.OnGoldChangedEvent += UpdateGoldUI;

            UpdateGoldUI();
        }

        private void UpdateGoldUI()
        {
            int newValue = goldTracker.Gold;
            int oldValue = int.Parse(goldText.text);
            if (newValue == oldValue) return;

            goldText.text = newValue.ToString("D2");

            TextEffect textEffect = goldText.gameObject.GetComponent<TextEffect>();
            if (textEffect != null)
            {
                if (newValue < oldValue)
                {
                    StartCoroutine(TextEffectRoutine(textEffect, "goldLoss"));
                }
                else if (newValue > oldValue)
                {
                    StartCoroutine(TextEffectRoutine(textEffect, "goldGain"));
                }
            }
        }

        private IEnumerator TextEffectRoutine(TextEffect textEffect, string tag)
        {
            textEffect.StartManualEffect(tag);
            textEffect.StartManualEffect("goldChanged");
            yield return new WaitForSecondsRealtime(0.4f);
            textEffect.StopManualEffects();
        }
    }
}