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
            string textString = $"State: {GameManager.Instance.StateMachine.CurrentSuperState} > {GameManager.Instance.StateMachine.CurrentState}";
            stateText.text = textString;
        }
    }

    public class DebugNodeProcessingUI : MonoBehaviour
    {
        [SerializeField] TMP_Text nodeProcessingText;

        void Update()
        {
            string textString = $"State: {GameManager.Instance.StateMachine.CurrentSuperState} > {GameManager.Instance.StateMachine.CurrentState}";
            nodeProcessingText.text = textString;
        }
    }
}

