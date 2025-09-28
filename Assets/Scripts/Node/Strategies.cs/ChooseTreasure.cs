using System.Collections.Generic;
using Project.Items;
using UnityEngine;

namespace Project.GameNode.Strategies
{
    [CreateAssetMenu(fileName = "NewChooseTreasure", menuName = "Nodes/ChooseTreasure", order = 1)]
    public class ChooseTreasure : ScriptableObject, INodeStrategy
    {
        bool hasBeenUsed = false;

        public Status Resolve(Node other)
        {
            throw new System.NotImplementedException();
        }

        public void Reset()
        {
            throw new System.NotImplementedException();
        }

        public void ResetNode()
        {
            hasBeenUsed = false;
        }
    }
}