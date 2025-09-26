using System;
using System.Collections.Generic;
using Project.Grid;
using Project.Hero;
using UnityEngine;

namespace Project
{
    public enum GameState
    {

    }

    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] HeroNode hero;
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

            List<Node> nodesToProcess = GridManager.Instance.GetNodesRegisteredToCell(hero.CurrentCell);
            foreach (Node node in nodesToProcess)
            {
                node.Process();
            }
        }

        public void StartGame()
        {
            Turn = 0;
            ProcessTurn();
        }
    }
}


