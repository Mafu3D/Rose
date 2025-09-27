using Project;
using TMPro;
using UnityEngine;

public class MovesRemainingUI : MonoBehaviour {
    [SerializeField] private TMP_Text movesRemainingText;

    void Start()
    {
        GameManager.Instance.Hero.OnRemainingMovesChanged += UpdateMovesTracker;
        UpdateMovesTracker();
    }

    private void UpdateMovesTracker()
    {
        movesRemainingText.text = GameManager.Instance.Hero.MovesRemaining.ToString();
    }
}