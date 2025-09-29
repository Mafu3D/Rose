using System;
using Project;
using TMPro;
using UnityEngine;

namespace Project.UI.MainUI
{
    public class TurnUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text turnText;

        void Start()
        {
            GameManager.Instance.OnRoundStartEvent += UpdateTurnTracker;
            UpdateTurnTracker();
        }

        private void UpdateTurnTracker()
        {
            turnText.text = $"Turn: {GameManager.Instance.Round.ToString("D3")}";
        }
    }
}