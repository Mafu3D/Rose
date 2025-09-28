using Project.GameNode;
using Project.GameNode.Strategies;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWait", menuName = "Nodes/Wait", order = 1)]
public class Wait : ScriptableObject, INodeStrategy
{
    [SerializeField] float timeToWait = 3f;
    float timer = 0f;
    public void Reset()
    {
        timer = 0f;
    }

    public void ResetNode()
    {
        Reset();
    }

    public Status Resolve(Node other)
    {
        if (timer > timeToWait)
        {
            return Status.Complete;
        }
        timer += Time.deltaTime;
        return Status.Running;
    }
}