using System.Collections.Generic;
using Project.Attributes;
using Project.GameplayEffects;
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

        [Header("Combat")]
        [SerializeField] public AttributesData AttributesData;

        [Header("Strategies")]
        // Resolved During Activate Phase
        [SerializeField] public List<GameplayEffectStrategy> OnActivateStrategies = new List<GameplayEffectStrategy>();

        // Resolved During Start Of Round Phase
        [SerializeField] public List<GameplayEffectStrategy> OnStartOfRoundStrategies = new List<GameplayEffectStrategy>();

        // Resolved During Player Move Phase
        [SerializeField] public List<GameplayEffectStrategy> OnPlayerEnterStrategies = new List<GameplayEffectStrategy>();
        [SerializeField] public List<GameplayEffectStrategy> OnPlayerExitStrategies = new List<GameplayEffectStrategy>();

        // Resolved During End Of Turn Phase
        [SerializeField] public List<GameplayEffectStrategy> OnEndOfTurnStratgies = new List<GameplayEffectStrategy>();

        // Resolved During End Of Round Phase
        [SerializeField] public List<GameplayEffectStrategy> OnEndOfRoundStrategies = new List<GameplayEffectStrategy>();

        // NOT YET IMPLEMENTED
        [SerializeField] public List<GameplayEffectStrategy> OnCreateStrategies = new List<GameplayEffectStrategy>();
        [SerializeField] public List<GameplayEffectStrategy> OnDestroyStrategies = new List<GameplayEffectStrategy>();
    }
}