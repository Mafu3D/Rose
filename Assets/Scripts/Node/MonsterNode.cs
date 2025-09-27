

using UnityEngine;

namespace Project.GameNode
{
    public class MonsterNode : Node
    {
        public override Status Process()
        {
            Debug.Log($"Monster attack: {NodeData.DisplayName}");
            return Status.Success;
        }

        public override void Reset()
        {
            // Noop
        }
    }
}