using System.Collections.Generic;
using Project.Items;
using UnityEngine;

namespace Project
{
    public class GameSettingsSetter : MonoBehaviour
    {
        [SerializeField] GameSettingsDefinition gameSettingsDefinition;

        [SerializeField] List<int> maps = new();
        [SerializeField] List<int> startHealths = new();
        [SerializeField] List<int> startGolds = new();
        [SerializeField] List<CharacterData> bosses = new();
        [SerializeField] List<int> bossRounds = new();
        [SerializeField] List<InventoryDefinition> inventories = new();
        [SerializeField] List<float> gameSpeeds = new();

        int mapIndex;
        int startHealthIndex;
        int bossesIndex;
        int preloadInventoryIndex;
        int gameSpeedsIndex;
        int startGoldIndex;
        int bossRoundsIndex;


        public void SetToDefault()
        {
            gameSettingsDefinition.Map = maps[0];
            gameSettingsDefinition.StartingHealth = startHealths[0];
            gameSettingsDefinition.BossData = bosses[0];
            gameSettingsDefinition.PreloadedInventory = inventories[0];
            gameSettingsDefinition.GameSpeed = gameSpeeds[0];
            gameSettingsDefinition.StartingGold = startGolds[0];

            gameSettingsDefinition.RoundsTillBoss = bossRounds[0];
        }

        public void IncMapIndex()
        {
            mapIndex += 1;
            if (mapIndex >= maps.Count)
            {
                mapIndex = 0;
            }
            gameSettingsDefinition.Map = maps[mapIndex];
        }

        public void IncStartHealths()
        {
            startHealthIndex += 1;
            if (startHealthIndex >= startHealths.Count)
            {
                startHealthIndex = 0;
            }
            gameSettingsDefinition.StartingHealth = startHealths[startHealthIndex];
        }

        public void IncBosses()
        {
            bossesIndex += 1;
            if (bossesIndex >= bosses.Count)
            {
                bossesIndex = 0;
            }
            gameSettingsDefinition.BossData = bosses[bossesIndex];
        }

        public void IncInventories()
        {
            preloadInventoryIndex += 1;
            if (preloadInventoryIndex >= inventories.Count)
            {
                preloadInventoryIndex = 0;
            }
            gameSettingsDefinition.PreloadedInventory = inventories[preloadInventoryIndex];
        }

        public void IncGameSpeed()
        {
            gameSpeedsIndex += 1;
            if (gameSpeedsIndex >= gameSpeeds.Count)
            {
                gameSpeedsIndex = 0;
            }
            gameSettingsDefinition.GameSpeed = gameSpeeds[gameSpeedsIndex];
        }

        public void IncStartGold()
        {
            startGoldIndex += 1;
            if (startGoldIndex >= startGolds.Count)
            {
                startGoldIndex = 0;
            }
            gameSettingsDefinition.StartingGold = startGolds[startGoldIndex];
        }

        public void IncBossRounds()
        {
            bossRoundsIndex += 1;
            if (bossRoundsIndex >= bossRounds.Count)
            {
                bossRoundsIndex = 0;
            }
            gameSettingsDefinition.RoundsTillBoss = bossRounds[bossRoundsIndex];
        }
    }
}