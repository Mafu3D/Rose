

using UnityEngine;

namespace Project.GameNode
{
    public class LocationNode : Node
    {
        public override Status Resolve()
        {
            Debug.Log($"I am a location: {NodeData.DisplayName}");
            return Status.Complete;
        }

        public override void Reset()
        {
            // Noop
        }
    }
}