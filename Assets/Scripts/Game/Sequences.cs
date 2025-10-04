using System.Collections.Generic;
using UnityEngine;

namespace Project.Sequences
{
    public enum Status
    {
        Running,
        Complete
    }

    public abstract class Node
    {
        public string Name { get; private set; }
        public GameManager GameManager { get; private set; }

        public Node(string name, GameManager gameManager)
        {
            this.Name = name;
            this.GameManager = gameManager;
        }
        public abstract Status Execute();
        public abstract void Enter();
        public abstract void Exit();
    }

    public class Sequencer
    {
        private List<Node> nodes = new();
        private int currentIndex = 0;
        private bool nodeEntered;

        public void AddNode(Node node)
        {
            nodes.Add(node);
        }

        public void Update()
        {
            if (nodes.Count == 0) return;

            if (!nodeEntered)
            {
                nodes[currentIndex].Enter();
                nodeEntered = true;
            }
            Status status = nodes[currentIndex].Execute();
            if (status == Status.Complete)
            {
                nodeEntered = false;
                nodes[currentIndex].Exit();
                currentIndex++;
                if (currentIndex >= nodes.Count)
                {
                    currentIndex = 0;
                }
            }
        }
    }
}