using Project.Grid;
using UnityEngine;

namespace Project.GameNode
{
    public class TestNode : Node
    {
        [SerializeField] float timeToProcess = 3f;
        float timeProcessing;
        public override Status Process()
        {
            timeProcessing += Time.deltaTime;
            if (timeProcessing < timeToProcess)
            {
                return Status.Running;
            }
            return Status.Success;
        }

        public override void Reset()
        {
            timeProcessing = 0f;
        }
    }
}
