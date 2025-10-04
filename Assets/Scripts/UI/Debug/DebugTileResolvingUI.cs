using Project.GameplayEffects;
using TMPro;
using UnityEngine;

namespace Project.UI.DebugUI
{
    public class DebugNodeResolvingUI : MonoBehaviour
    {
        [SerializeField] TMP_Text tileProcessingText;

        void Update()
        {
            string textString = "Resolving: ";

            if (GameManager.Instance.EffectQueue.ResolvingQueue)
            {
                GameplayEffectStrategy effect = GameManager.Instance.EffectQueue.GetCurrentEffect();
                if (effect != null)
                {
                    textString += effect.name;
                }
                else
                {
                    textString += "None";
                }
            }
            else
            {
                textString += "None";
            }
            tileProcessingText.text = textString;
        }
    }
}

