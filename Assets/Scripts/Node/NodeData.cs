using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.GameNode
{
    [CreateAssetMenu(fileName = "NodeData", menuName = "NodeData", order = 0)]
    public class NodeData : SerializedScriptableObject
    {
        [SerializeField] public string DisplayName;
        [SerializeField] public Sprite Sprite;
        [SerializeField] public int Priority = 0;
        [SerializeField] public NodeType NodeType;
    }
}