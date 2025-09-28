using Project.GameNode;
using Project.GameNode.Strategies;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWait", menuName = "Nodes/Wait", order = 0)]
public class Wait : ScriptableObject, IStrategy
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