using System;
using UnityEngine;

namespace Project
{
    public enum GameState
    {

    }

    public class GameManager : Singleton<GameManager>
    {
        public int Turn { get; private set; }

        public event Action ProcessTurnEvent;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            StartGame();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ProcessTurn()
        {
            Turn += 1;
            ProcessTurnEvent?.Invoke();
            // Debug.Log($"Turn : {Turn}");
        }

        public void StartGame()
        {
            Turn = 0;
            ProcessTurn();
        }
    }
}


