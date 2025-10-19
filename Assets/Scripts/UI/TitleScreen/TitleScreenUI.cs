using Project.SceneManagement;
using TMPro;
using UnityEngine;

namespace Project.UI.TitleScreen
{
    public class TitleScreenUI : MonoBehaviour
    {
        [SerializeField] TMP_Text debugPregameSettingsText;
        [SerializeField] GameSettingsDefinition gameSettings;

        public void StartGame()
        {
            Debug.Log("Game Started");
            int mapIndex;
            if (gameSettings.Map == 0)
            {
                mapIndex = Random.Range(1, 4);
            }
            else
            {
                mapIndex = gameSettings.Map;
            }
            SceneLoader.Load(mapIndex + 1);
        }

        public void ExitGame()
        {
            Debug.Log("Exit Game");
            SceneLoader.Quit();
        }

        void Update()
        {
            UpdateSettingsText();
        }

        void Awake()
        {
            debugPregameSettingsText.gameObject.SetActive(false);
        }

        public void TogglePregameSettingsDebug()
        {
            if (debugPregameSettingsText.gameObject.activeSelf) debugPregameSettingsText.gameObject.SetActive(false);
            else debugPregameSettingsText.gameObject.SetActive(true);
        }

        public void UpdateSettingsText()
        {
            string text = "";
            if (gameSettings.Map == 0) text += $"Map: Random \n";
            else text += $"Map: {gameSettings.Map} \n";

            if (gameSettings.StartingHealth == 0) text += $"Start Health: Default \n";
            else text += $"Start Health: {gameSettings.StartingHealth} \n";

            if (gameSettings.StartingGold == 0) text += $"Start Gold: Default \n";
            else text += $"Start Gold: {gameSettings.StartingGold} \n";

            if (gameSettings.StartingSpeed == 0) text += $"Start Speed: Default \n";
            else text += $"Start Speed: {gameSettings.StartingSpeed} \n";

            if (gameSettings.StartingGold == 0) text += $"Start Gold: Default \n";
            else text += $"Start Gold: {gameSettings.StartingGold} \n";

            if (gameSettings.BossData == null) text += $"Boss: Random \n";
            else text += $"Boss: {gameSettings.BossData.DisplayName} \n";

            if (gameSettings.RoundsTillBoss == 0) text += $"Rounds: Default \n";
            else text += $"Rounds: {gameSettings.RoundsTillBoss - 1} \n";

            if (gameSettings.PreloadedInventory == null) text += $"Starting Inventory: Default \n";
            else text += $"Starting Inventory: {gameSettings.PreloadedInventory} \n";

            if (gameSettings.GameSpeed == 0) text += $"GameSpeed: Default \n";
            else text += $"GameSpeed: {gameSettings.GameSpeed} \n";

            debugPregameSettingsText.text = text;
        }
    }
}