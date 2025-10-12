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

        [Header("Rewards")]
        [SerializeField] public int gemReward = 1;
        [SerializeField] public int goldReward = 3;

        [Header("Default Inventory")]
        [SerializeField] public List<ItemData> DefaultInventory;
    }
}