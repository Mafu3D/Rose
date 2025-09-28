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

        public override void Reset() { }

        public override Status Resolve() => Status.Complete;

        public override Status Start()
        {
            GameObject gameObject = Instantiate(nodePrefab, GameManager.Instance.Hero.CurrentCell.Center, Quaternion.identity);
            Node node = gameObject.GetComponent<Node>();
            node.RegisterToGrid();
            return Status.Complete;
        }
    }
}