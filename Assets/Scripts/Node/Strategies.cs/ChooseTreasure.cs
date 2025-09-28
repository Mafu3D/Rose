using UnityEngine;

namespace Project.GameNode.Strategies
{
    [CreateAssetMenu(fileName = "NewChooseTreasure", menuName = "Nodes/ChooseTreasure", order = 1)]
    public class ChooseTreasure : ScriptableObject, INodeStrategy
    {
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
            throw new System.NotImplementedException();
        }
    }
}