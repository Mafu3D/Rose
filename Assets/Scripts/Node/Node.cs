using System;
using System.Collections.Generic;
using Project.GameNode.Strategies;
using Project.Grid;
using UnityEngine;

namespace Project.GameNode
{
    public enum NodeType
    {
        Location,
        NPC,
        Hero,
        Combatant,
        Event
    }

    public enum Status
    {
        Running,
        Complete
    }

    public abstract class Node : MonoBehaviour
    {
        public Cell CurrentCell;
        private SpriteRenderer mySpriteRenderer;

        [SerializeField] public NodeData NodeData;

        List<Node> usedNodes = new();

        protected virtual void Awake()
        {
            mySpriteRenderer = GetComponent<SpriteRenderer>();
            mySpriteRenderer.sprite = NodeData.Sprite;
        }

        protected virtual void Start()
        {
            RegisterToGrid();
            ResetNode();
        }

        private void ResetNode()
        {
            if (NodeData.OnTurnResolveStrategies != null) foreach (INodeStrategy strategy in NodeData.OnTurnResolveStrategies) strategy.Reset();
            if (NodeData.OnPlayerEnterStrategies != null) foreach (INodeStrategy strategy in NodeData.OnPlayerEnterStrategies) strategy.Reset();
            if (NodeData.OnPlayerExitStrategies != null) foreach (INodeStrategy strategy in NodeData.OnPlayerExitStrategies) strategy.Reset();
            if (NodeData.OnCreateStrategies != null) foreach (INodeStrategy strategy in NodeData.OnCreateStrategies) strategy.Reset();
            if (NodeData.OnDestroyStrategies != null) foreach (INodeStrategy strategy in NodeData.OnDestroyStrategies) strategy.Reset();
            if (NodeData.OnRoundStartStrategies != null) foreach (INodeStrategy strategy in NodeData.OnRoundStartStrategies) strategy.Reset();
            if (NodeData.OnPlayerTurnEndStrategies != null) foreach (INodeStrategy strategy in NodeData.OnPlayerTurnEndStrategies) strategy.Reset();
            if (NodeData.OnRoundEndStrategies != null) foreach (INodeStrategy strategy in NodeData.OnRoundEndStrategies) strategy.Reset();
            usedNodes = new();
        }

        public void RegisterToGrid()
        {
            CurrentCell = GameManager.Instance.Grid.WorldPositionToCell(this.transform.position);
            GameManager.Instance.Grid.RegisterToCell(CurrentCell, this);
        }

        List<INodeStrategy> strategiesToResolve = new();
        int resolvingStrategyIndex;
        bool isCurrentlyResolvingStrategies;

        public INodeStrategy ResolvingStrategy
        {
            get
            {
                if (strategiesToResolve!= null && strategiesToResolve.Count > 0)
                {
                    return strategiesToResolve[resolvingStrategyIndex];
                }
                return null;
            }
        }

        public virtual Status OnPlayerEnter()
        {
            return ResolveStrategies(NodeData.OnPlayerEnterStrategies);
        }

        public virtual Status OnTurnResolve()
        {
            return ResolveStrategies(NodeData.OnTurnResolveStrategies);
        }

        private Status ResolveStrategies(List<INodeStrategy> strategies)
        {
            if (!NodeData.CanBeUsedMultipleTimes && usedNodes.Contains(GameManager.Instance.Player.HeroNode)) return Status.Complete;

            if (!isCurrentlyResolvingStrategies)
            {
                strategiesToResolve = strategies;
                resolvingStrategyIndex = 0;
                isCurrentlyResolvingStrategies = true;
            }

            if (strategiesToResolve == null) return Status.Complete;

            while (resolvingStrategyIndex < strategiesToResolve.Count)
            {
                Status status = strategiesToResolve[resolvingStrategyIndex].Resolve(this);
                Debug.Log($"Resolving: {strategiesToResolve[resolvingStrategyIndex]}");
                if (status != Status.Complete)
                {
                    return status;
                }
                strategiesToResolve[resolvingStrategyIndex].Reset();
                resolvingStrategyIndex++;
            }

            resolvingStrategyIndex = 0;
            strategiesToResolve = new();
            isCurrentlyResolvingStrategies = false;

            usedNodes.Add(GameManager.Instance.Player.HeroNode);

            if (NodeData.DestroyAfterUsing)
            {
                Destroy(this.gameObject);
            }
            return Status.Complete;
        }

        public void ResetStrategies(List<INodeStrategy> strategies)
        {

        }
    }
}
