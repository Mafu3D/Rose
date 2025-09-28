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
            if (NodeData.OnTurnResolveStrategies != null) foreach (INodeStrategy strategy in NodeData.OnTurnResolveStrategies) strategy.ResetNode();
            if (NodeData.OnPlayerEnterStrategies != null) foreach (INodeStrategy strategy in NodeData.OnPlayerEnterStrategies) strategy.ResetNode();
            if (NodeData.OnPlayerExitStrategies != null) foreach (INodeStrategy strategy in NodeData.OnPlayerExitStrategies) strategy.ResetNode();
            if (NodeData.OnCreateStrategies != null) foreach (INodeStrategy strategy in NodeData.OnCreateStrategies) strategy.ResetNode();
            if (NodeData.OnDestroyStrategies != null) foreach (INodeStrategy strategy in NodeData.OnDestroyStrategies) strategy.ResetNode();
            if (NodeData.OnRoundStartStrategies != null) foreach (INodeStrategy strategy in NodeData.OnRoundStartStrategies) strategy.ResetNode();
            if (NodeData.OnPlayerTurnEndStrategies != null) foreach (INodeStrategy strategy in NodeData.OnPlayerTurnEndStrategies) strategy.ResetNode();
            if (NodeData.OnRoundEndStrategies != null) foreach (INodeStrategy strategy in NodeData.OnRoundEndStrategies) strategy.ResetNode();
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
            return Status.Complete;
        }

        public void ResetStrategies(List<INodeStrategy> strategies)
        {

        }
    }
}
