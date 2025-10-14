using System.Collections.Generic;
using Project.Attributes;
using Project.Items;
using UnityEngine;


namespace Project
{

    [CreateAssetMenu(fileName = "NewCharacterData", menuName = "Character Data", order = 0)]
    public class CharacterData : ScriptableObject
    {
        [SerializeField] public string DisplayName;
        [SerializeField] public string Description;
        [SerializeField] public string CombatDescription;
        [SerializeField] public Sprite CombatSprite;
        [SerializeField] public AttributesData AttributesData;
        [SerializeField] public InventoryDefinition InventoryDefinition;

        [Header("Rewards")]
        [SerializeField] public int gemReward = 1;
        [SerializeField] public int goldReward = 3;

        [Header("Immunity")]
        [SerializeField] public bool ImmuneToFrost = false;
        [SerializeField] public bool ImmuneToBurn = false;
        [SerializeField] public bool ImmuneToVulnerable = false;
        [SerializeField] public bool ImmuneToWeaken = false;
    }
}