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
            foreach (IStrategy strategy in NodeData.OnTurnResolveStrategies) strategy.ResetNode();
            foreach (IStrategy strategy in NodeData.OnPlayerEnterStrategies) strategy.ResetNode();
            foreach (IStrategy strategy in NodeData.OnPlayerExitStrategies) strategy.ResetNode();
            foreach (IStrategy strategy in NodeData.OnCreateStrategies) strategy.ResetNode();
            foreach (IStrategy strategy in NodeData.OnDestroyStrategies) strategy.ResetNode();
            foreach (IStrategy strategy in NodeData.OnRoundStartStrategies) strategy.ResetNode();
            foreach (IStrategy strategy in NodeData.OnPlayerTurnEndStrategies) strategy.ResetNode();
            foreach (IStrategy strategy in NodeData.OnRoundEndStrategies) strategy.ResetNode();
        }

        public void RegisterToGrid()
        {
            CurrentCell = GameManager.Instance.Grid.WorldPositionToCell(this.transform.position);
            GameManager.Instance.Grid.RegisterToCell(CurrentCell, this);
        }

        List<IStrategy> strategiesToResolve = new();
        int resolvingStrategyIndex;
        bool isCurrentlyResolvingStrategies;

        public IStrategy ResolvingStrategy
        {
            get
            {
                if (strategiesToResolve.Count > 0)
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

        private Status ResolveStrategies(List<IStrategy> strategies)
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

        public void ResetStrategies(List<IStrategy> strategies)
        {

        }
    }
}
