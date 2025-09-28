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
            RegisterToGrid();
            ResetNode();
        }

        private void ResetNode()
        {
            if (NodeData.OnEndTurnStrategies != null) foreach (GameplayEffectStrategy strategy in NodeData.OnEndTurnStrategies) strategy.Reset();
            if (NodeData.OnPlayerEnterStrategies != null) foreach (GameplayEffectStrategy strategy in NodeData.OnPlayerEnterStrategies) strategy.Reset();
            if (NodeData.OnPlayerExitStrategies != null) foreach (GameplayEffectStrategy strategy in NodeData.OnPlayerExitStrategies) strategy.Reset();
            if (NodeData.OnCreateStrategies != null) foreach (GameplayEffectStrategy strategy in NodeData.OnCreateStrategies) strategy.Reset();
            if (NodeData.OnDestroyStrategies != null) foreach (GameplayEffectStrategy strategy in NodeData.OnDestroyStrategies) strategy.Reset();
            if (NodeData.OnRoundStartStrategies != null) foreach (GameplayEffectStrategy strategy in NodeData.OnRoundStartStrategies) strategy.Reset();
            if (NodeData.OnPlayerTurnEndStrategies != null) foreach (GameplayEffectStrategy strategy in NodeData.OnPlayerTurnEndStrategies) strategy.Reset();
            if (NodeData.OnRoundEndStrategies != null) foreach (GameplayEffectStrategy strategy in NodeData.OnRoundEndStrategies) strategy.Reset();
            usedNodes = new();
        }

        public void RegisterToGrid()
        {
            CurrentCell = GameManager.Instance.Grid.WorldPositionToCell(this.transform.position);
            GameManager.Instance.Grid.RegisterToCell(CurrentCell, this);
            Debug.Log($"Registering {this} to Cell {CurrentCell.ToString()}");
        }

        public void OnPlayerEnter()
        {
            ExecuteStrategies(NodeData.OnPlayerEnterStrategies);
        }

        public void ExecuteOnEndTurn()
        {
            ExecuteStrategies(NodeData.OnEndTurnStrategies);
        }

        private void ExecuteStrategies(List<GameplayEffectStrategy> strategies)
        {
            foreach (GameplayEffectStrategy effect in strategies)
            {
                GameManager.Instance.EffectQueue.AddEffect(effect);
            }
        }

        public void ResetStrategies(List<GameplayEffectStrategy> strategies)
        {

        }
    }
}
