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
            int turnsRemaining = GameManager.Instance.RoundsTillBoss - GameManager.Instance.Round;

            string hexColor;
            if (turnsRemaining > 10)
            {
                hexColor = "#FFFFFF";
            }
            else if (turnsRemaining > 5)
            {
                hexColor = "#FF4C00";
            }
            else
            {
                hexColor = "#FB0404";
            }
            Color color;
            if (ColorUtility.TryParseHtmlString(hexColor, out color))
            {
                turnText.color = color;
            }
            turnText.text = $"Turns Till Boss: {turnsRemaining.ToString("D2")}";
        }
    }
}