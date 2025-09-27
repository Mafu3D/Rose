using Project;
using TMPro;
using UnityEngine;

public class DebugStateUI : MonoBehaviour
{
    [SerializeField] TMP_Text stateText;

    void Update()
    {
        string textString = $"State: {GameManager.Instance.StateMachine.CurrentSuperState} > {GameManager.Instance.StateMachine.CurrentState}";
        stateText.text = textString;
    }
}
