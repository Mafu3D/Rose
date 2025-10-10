using Project.GameplayEffects;
using TMPro;
using UnityEngine;

namespace Project.UI.DebugUI
{
    public class DebugGameplayEffectQueue : MonoBehaviour
    {
        [SerializeField] TMP_Text queueText;

        void Update()
        {
            string outputString = "";
            foreach(GameplayEffectStrategy effect in GameManager.Instance.EffectQueue.Queue)
            {
                outputString += $"{effect.name} \n";
            }
            queueText.text = outputString;
        }
    }
}

