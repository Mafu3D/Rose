using System;
using System.Collections.Generic;
using Project.Attributes;
using Project.GameplayEffects;
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

    public class Node : MonoBehaviour
    {
        public Cell CurrentCell;
        private SpriteRenderer mySpriteRenderer;

        [SerializeField] public NodeData NodeData;
        public CharacterAttributes Attributes { get; private set; }

        List<Node> usedNodes = new();

        void OnValidate()
        {
            if (NodeData != null)
            {
                mySpriteRenderer = GetComponent<SpriteRenderer>();
                mySpriteRenderer.sprite = NodeData.Sprite;
            }
        }

        protected virtual void Awake()
        {
            mySpriteRenderer = GetComponent<SpriteRenderer>();
            mySpriteRenderer.sprite = NodeData.Sprite;

            if (NodeData.AttributesData != null)
            {
                Attributes = new CharacterAttributes(NodeData.AttributesData);
            }
        }

        protected virtual void Start()
        {
            GameManager.Instance.OnGameStartEvent += Initialize;
        }

        private void Initialize()
        {
            RegisterToGrid();
            ResetNode();
        }

        private void ResetNode()
        {
            // if (NodeData.OnRoundStartStrategies != null) foreach (GameplayEffectStrategy strategy in NodeData.OnRoundStartStrategies) strategy.ResetEffect();
            // if (NodeData.OnTurnStartStrategies != null) foreach (GameplayEffectStrategy strategy in NodeData.OnTurnStartStrategies) strategy.ResetEffect();
            // if (NodeData.OnPlayerMoveStrategies != null) foreach (GameplayEffectStrategy strategy in NodeData.OnPlayerMoveStrategies) strategy.ResetEffect();
            // if (NodeData.OnPlayerEnterStrategies != null) foreach (GameplayEffectStrategy strategy in NodeData.OnPlayerEnterStrategies) strategy.ResetEffect();
            // if (NodeData.OnPlayerExitStrategies != null) foreach (GameplayEffectStrategy strategy in NodeData.OnPlayerExitStrategies) strategy.ResetEffect();
            // if (NodeData.OnEndOfTurnStrategies != null) foreach (GameplayEffectStrategy strategy in NodeData.OnEndOfTurnStrategies) strategy.ResetEffect();
            // if (NodeData.OnCreateStrategies != null) foreach (GameplayEffectStrategy strategy in NodeData.OnCreateStrategies) strategy.ResetEffect();
            // if (NodeData.OnDestroyStrategies != null) foreach (GameplayEffectStrategy strategy in NodeData.OnDestroyStrategies) strategy.ResetEffect();
            // if (NodeData.OnRoundEndStrategies != null) foreach (GameplayEffectStrategy strategy in NodeData.OnRoundEndStrategies) strategy.ResetEffect();
            usedNodes = new();
        }

        public void RegisterToGrid()
        {
            CurrentCell = GameManager.Instance.Grid.WorldPositionToCell(this.transform.position);
            GameManager.Instance.Grid.RegisterToCell(CurrentCell, this);
            Debug.Log($"Registering {this} to Cell {CurrentCell.ToString()}");
        }

        public void ResetStrategies(List<GameplayEffectStrategy> strategies)
        {

        }
    }
}
