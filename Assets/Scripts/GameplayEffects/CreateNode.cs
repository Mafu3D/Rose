using System.Collections.Generic;
using Project.GameNode;
using Project.Items;
using UnityEngine;

namespace Project.GameplayEffects
{
    // Add options for creating at hero, at a nearby cell, at a specific cell, etc.

    [CreateAssetMenu(fileName = "NewCreateNode", menuName = "Effects/Create Node", order = 1)]
    public class CreateNode : GameplayEffectStrategy
    {
        [SerializeField] GameObject nodePrefab;
        // [SerializeField] NodeData nodeData;

        public override void Reset() { }

        public override Status Resolve() => Status.Complete;

        public override Status Start()
        {
            // if (nodeData != null)
            // {
            //     // TODO: Move to node factory
            //     GameObject nodeObject = new GameObject(nodeData.DisplayName);
            //     nodeObject.transform.position = GameManager.Instance.Hero.CurrentCell.Center;
            //     SpriteRenderer spriteRenderer = nodeObject.AddComponent<SpriteRenderer>();
            //     Node node = nodeObject.AddComponent<Node>();
            //     spriteRenderer.sprite = nodeData.Sprite;
            //     node.NodeData = nodeData;
            //     node.RegisterToGrid();
            // }

            GameObject gameObject = Instantiate(nodePrefab, GameManager.Instance.Hero.CurrentCell.Center, Quaternion.identity);
            Node node = gameObject.GetComponent<Node>();
            node.RegisterToGrid();
            return Status.Complete;
        }
    }
}