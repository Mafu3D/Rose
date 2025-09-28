using Project.GameNode;
using Project.GameNode.Strategies;
using Project.GameplayEffects;
using Project.GameStates;
using Project.States;
using TMPro;
using UnityEngine;

namespace Project.UI.DebugUI
{
    public class DebugNodeResolvingUI : MonoBehaviour
    {
        [SerializeField] TMP_Text nodeProcessingText;

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
            nodeProcessingText.text = textString;
        }
    }
}

