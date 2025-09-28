using System.Collections.Generic;
using Project.Attributes;
using Project.GameNode.Strategies;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.GameNode
{
    [CreateAssetMenu(fileName = "NodeData", menuName = "Nodes/NodeData", order = 0)]
    public class NodeData : SerializedScriptableObject
    {
        [Header("Meta")]
        [SerializeField] public string DisplayName;
        [SerializeField] public string Description;
        [SerializeField] public Sprite Sprite;
        [SerializeField] public int Priority = 0;
        [SerializeField] public NodeType NodeType;

        [Header("Usability")]
        [SerializeField] public bool CanBeUsedMultipleTimes = false;
        [SerializeField] public bool DestroyAfterUsing = false;

        [Header("Strategies")]
        [SerializeField] public List<INodeStrategy> OnTurnResolveStrategies = new List<INodeStrategy>();
        [SerializeField] public List<INodeStrategy> OnPlayerEnterStrategies = new List<INodeStrategy>();
        [SerializeField] public List<INodeStrategy> OnPlayerExitStrategies = new List<INodeStrategy>();
        [SerializeField] public List<INodeStrategy> OnCreateStrategies = new List<INodeStrategy>();
        [SerializeField] public List<INodeStrategy> OnDestroyStrategies = new List<INodeStrategy>();
        [SerializeField] public List<INodeStrategy> OnRoundStartStrategies = new List<INodeStrategy>();
        [SerializeField] public List<INodeStrategy> OnPlayerTurnEndStrategies = new List<INodeStrategy>();
        [SerializeField] public List<INodeStrategy> OnRoundEndStrategies = new List<INodeStrategy>();
    }
}