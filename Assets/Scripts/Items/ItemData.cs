using UnityEngine;

namespace Project.Items
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "ItemData", order = 0)]
    public class ItemData : ScriptableObject
    {
        [SerializeField] public int HealthModifier = 0;
        [SerializeField] public int SpeedModifier = 0;
        [SerializeField] public int StrengthModifier = 0;
        [SerializeField] public int MagicModifier = 0;
        [SerializeField] public int DexterityModifier = 0;
        [SerializeField] public int ArmorModifier = 0;

        // On Combat Start Strategies

        // On Combat End Strategies

        // On Hit Strategies

        // On Receive Damage Strategies

        // On SelfBloodied Strategies

        // On SelfExposed Strategies

        // On OpponentBloodied Strategies

        // On OpponentExposed Strategies

        // On Turn Start Strategies

        // On Turn End Strategies

        // On SelfDeath Strategies

        // On OpponentDeath Strategies
    }
}