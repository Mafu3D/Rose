using System.Collections.Generic;
using Project.Items;
using UnityEngine;

namespace Project
{
    [CreateAssetMenu(fileName = "NewGameSettingsDefinition", menuName = "Game Settings", order = 1)]

    public class GameSettingsDefinition : ScriptableObject
    {
        [SerializeField] public int Map;
        [SerializeField] public int StartingHealth = 0;
        [SerializeField] public int StartingGold = 0;
        [SerializeField] public CharacterData BossData;
        [SerializeField] public InventoryDefinition PreloadedInventory;
        [SerializeField] public float GameSpeed;
        [SerializeField] public int RoundsTillBoss;
    }
}