using System;
using System.Collections.Generic;
using Project.Grid;
using Project.Hero;
using Project.GameNode;
using UnityEngine;

namespace Project
{
    public enum GameState
    {
        PlayerMove,
        ProcessingTurn,
    }

    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] HeroNode hero;
        public int Turn { get; private set; }

        public GameState GameState { get; private set; } = GameState.PlayerMove;

        public event Action OnStartPlayerTurn;
        public event Action OnEndPlayerTurn;
        public event Action OnEndTurn;

        private List<Node> nodesToProcess;
        private int currentProcessingNode = 0;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            StartGame();
        }

        // Update is called once per frame
        void Update()
        {
            if (GameState == GameState.ProcessingTurn)
            {
                Status status = ProcessTurn();
                if (status == Status.Success)
                {
                    currentProcessingNode = 0;
                    EndTurn();
                }
            }
        }

        public Status ProcessTurn()
        {
            while (currentProcessingNode < nodesToProcess.Count)
            {
                Status status = nodesToProcess[currentProcessingNode].Process();
                if (status != Status.Success)
                {
                    return status;
                }
                nodesToProcess[currentProcessingNode].Reset();
                currentProcessingNode++;
            }
            return Status.Success;
        }

        public void EndPlayerTurn()
        {
            OnEndPlayerTurn?.Invoke();
            nodesToProcess = GridManager.Instance.GetNodesRegisteredToCell(hero.CurrentCell);
            GameState = GameState.ProcessingTurn;

            foreach (Node node in nodesToProcess)
            {
                node.Process();
            }

        }

        private void EndTurn()
        {
            Turn += 1;
            GameState = GameState.PlayerMove;
            OnEndTurn?.Invoke();
        }

        public void StartGame()
        {
            Turn = 0;
            EndPlayerTurn();
        }
    }
}


