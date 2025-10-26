using System.Collections.Generic;
using Project.Combat.CombatActions;
using Project.GameplayEffects;
using UnityEngine;

namespace Project.Items
{


    [CreateAssetMenu(fileName = "ItemData", menuName = "ItemData", order = 0)]
    public class ItemData : ScriptableObject
    {
        [Header("Meta")]
        [SerializeField] public string Name;
        [SerializeField] public string Description;
        [SerializeField] public string ItemID;
        [SerializeField] public Sprite Sprite;
        [SerializeField] public int GoldValue;
        [SerializeField] public ItemRarity Rarity;
        [SerializeField] public ItemType ItemType;
        [SerializeField] public bool UsableInOverworld;

        [Header("Modifiers")]
        [SerializeField] public int HealthModifier = 0;
        [SerializeField] public int MaxHealthModifier = 0;
        [SerializeField] public int ArmorModifier = 0;
        [SerializeField] public int MaxArmorModifier = 0;
        [SerializeField] public int StrengthModifier = 0;
        [SerializeField] public int MaxStrengthModifier = 0;
        [SerializeField] public int MagicModifier = 0;
        [SerializeField] public int MaxMagicModifier = 0;
        [SerializeField] public int DexterityModifier = 0;
        [SerializeField] public int MaxDexterityModifier = 0;
        [SerializeField] public int SpeedModifier = 0;
        [SerializeField] public int MaxSpeedModifier = 0;

        [Header("Hidden attributes")]
        [SerializeField] public int MultistrikeModifier = 0;

        [Header("Uses")]
        [SerializeField] public int UsesPerCombat = -1;


        [Header("Combat Strategies")]

        [SerializeField] public List<CombatActionBaseData> OnHitStrategies = new();
        [SerializeField] public List<CombatActionBaseData> OnReceiveHitStrategies = new();

        [SerializeField] public List<CombatActionBaseData> OnCombatStartStrategies = new();
        [SerializeField] public List<CombatActionBaseData> OnCombatEndStrategies = new();

        [SerializeField] public List<CombatActionBaseData> OnRoundStartStrategies = new();
        [SerializeField] public List<CombatActionBaseData> OnRoundEndStrategies = new();

        [SerializeField] public List<CombatActionBaseData> OnTurnStartStrategies = new();
        [SerializeField] public List<CombatActionBaseData> OnTurnEndStrategies = new();

        [SerializeField] public List<CombatActionBaseData> OnEnemyTurnStartStrategies = new();
        [SerializeField] public List<CombatActionBaseData> OnEnemyTurnEndStrategies = new();


        [SerializeField] public List<CombatActionBaseData> OnSelfBloodiedStrategies = new();
        [SerializeField] public List<CombatActionBaseData> OnEnemyBloodiedStrategies = new();

        [SerializeField] public List<CombatActionBaseData> OnSelfExposedStrategies = new();
        [SerializeField] public List<CombatActionBaseData> OnEnemyExposedStrategies = new();

        [SerializeField] public List<CombatActionBaseData> OnDieStrategies = new();


        [Header("Consumable Strategies")]
        [SerializeField] public List<CombatActionBaseData> OnCombatUse = new();
        [SerializeField] public List<GameplayEffectStrategy> OnOverworldUse = new();

        // gameplay effects for defeat opponent, run, or steal?
    }
}