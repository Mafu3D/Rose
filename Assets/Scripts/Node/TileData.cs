using System.Collections.Generic;
using Project.Attributes;
using Project.GameplayEffects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.GameTiles
{
    [CreateAssetMenu(fileName = "TileData", menuName = "Tiles/Tile Data", order = 0)]
    public class TileData : SerializedScriptableObject
    {
        [Header("Meta")]
        [SerializeField] public string DisplayName;
        [SerializeField] public string Description;
        [SerializeField] public Sprite Sprite;
        [SerializeField] public int ActivationPriority = 0;
        [SerializeField] public TileType TileType;

        [Header("Usability")]
        [SerializeField] public bool CanBeUsedMultipleTimes = false;
        [SerializeField] public int UsesPerTurn = 1;
        [SerializeField] public bool LimitTotalUses = false;
        [SerializeField] public int TotalUses = 1;
        [SerializeField] public bool DestroyAfterUsing = false;

        [Header("Character")]
        [SerializeField] public CharacterData CharacterData;

        [Header("Strategies")]
        [SerializeField] public List<GameplayEffectStrategy> OnActivateStrategies = new List<GameplayEffectStrategy>();
        [SerializeField] public List<GameplayEffectStrategy> OnRoundStartStrategies = new List<GameplayEffectStrategy>();
        [SerializeField] public List<GameplayEffectStrategy> OnTurnStartStrategies = new List<GameplayEffectStrategy>();
        [SerializeField] public List<GameplayEffectStrategy> OnPlayerMoveStrategies = new List<GameplayEffectStrategy>();
        [SerializeField] public List<GameplayEffectStrategy> OnPlayerMoveEndStrategies = new List<GameplayEffectStrategy>();
        [SerializeField] public List<GameplayEffectStrategy> OnPlayerEnterStrategies = new List<GameplayEffectStrategy>();
        [SerializeField] public List<GameplayEffectStrategy> OnPlayerExitStrategies = new List<GameplayEffectStrategy>();
        [SerializeField] public List<GameplayEffectStrategy> OnEndOfTurnStrategies = new List<GameplayEffectStrategy>();
        [SerializeField] public List<GameplayEffectStrategy> OnDrawCardStrategies = new List<GameplayEffectStrategy>();
        [SerializeField] public List<GameplayEffectStrategy> OnEndOfRoundStrategies = new List<GameplayEffectStrategy>();



        [SerializeField] public List<GameplayEffectStrategy> OnCreateStrategies = new List<GameplayEffectStrategy>();
        [SerializeField] public List<GameplayEffectStrategy> OnDestroyStrategies = new List<GameplayEffectStrategy>();
    }
}