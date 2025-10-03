using System.Collections.Generic;
using UnityEngine;

namespace Sandbox
{
    public enum SequenceNodeStatus
    {
        Running,
        Complete
    }

    public abstract class Node
    {
        public Node next;
        public Node(Node next)
        {
            this.next = next;
        }

        public abstract SequenceNodeStatus Execute();
        public abstract void Enter();
        public abstract void Exit();


    }

    public class Sequence
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
            if (!nodeEntered)
            {
                nodes[currentIndex].Enter();
                nodeEntered = true;
            }
            SequenceNodeStatus status = nodes[currentIndex].Execute();
            if (status == SequenceNodeStatus.Complete)
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

    public class TestNodeA : Node
    {
        float timer;
        float time = 2f;

        public TestNodeA(Node next) : base(next) { }

        public override void Enter()
        {
            timer = 0f;
            Debug.Log("entering test node a");
        }

        public override SequenceNodeStatus Execute()
        {
            Debug.Log("executing test node a");
            timer += Time.deltaTime;
            if (timer > time)
            {
                return SequenceNodeStatus.Complete;
            }
            return SequenceNodeStatus.Running;
        }

        public override void Exit()
        {
            Debug.Log("exit test node a");
        }
    }

    public class TestNodeB : Node
    {
        float timer;
        float time = 2f;

        public TestNodeB(Node next) : base(next) { }

        public override void Enter()
        {
            timer = 0f;
            Debug.Log("entering test node b");
        }

        public override SequenceNodeStatus Execute()
        {
            Debug.Log("executing test node b");
            timer += Time.deltaTime;
            if (timer > time)
            {
                return SequenceNodeStatus.Complete;
            }
            return SequenceNodeStatus.Running;
        }

        public override void Exit()
        {
            Debug.Log("exit test node b");
        }
    }
}