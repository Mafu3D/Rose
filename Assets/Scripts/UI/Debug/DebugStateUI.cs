using Project;
using TMPro;
using UnityEngine;

namespace Project.UI.DebugUI
{
    public class DebugStateUI : MonoBehaviour
    {
        [SerializeField] TMP_Text stateText;

        void Update()
        {
            string textString = $"State: {GameManager.Instance.StateMachine.CurrentState.Name}";
            stateText.text = textString;
        }
    }
}

