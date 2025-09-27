

using UnityEngine;

namespace Project.GameNode
{
    public class LocationNode : Node
    {
        public override Status Process()
        {
            Debug.Log($"I am a location: {NodeData.DisplayName}");
            return Status.Success;
        }

        public override void Reset()
        {
            // Noop
        }
    }
}