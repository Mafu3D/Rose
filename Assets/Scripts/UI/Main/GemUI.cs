using System;
using System.Collections;
using EasyTextEffects;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Project.UI.MainUI
{
    public class GemUI : MonoBehaviour
    {
        [SerializeField] TMP_Text gemText;
        [SerializeField] GemTracker gemTracker;

        void Start()
        {
            gemTracker.OnGemChangedEvent += UpdateGemUI;

            UpdateGemUI();
        }

        private void UpdateGemUI()
        {
            int newValue = gemTracker.Gem;
            int oldValue = int.Parse(gemText.text);
            if (newValue == oldValue) return;

            gemText.text = newValue.ToString("D2");

            TextEffect textEffect = gemText.gameObject.GetComponent<TextEffect>();
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