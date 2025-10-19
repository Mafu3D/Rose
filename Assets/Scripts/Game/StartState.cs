using Project.UI.TitleScreen;
using UnityEngine;

namespace Project
{
    public class StartState : MonoBehaviour
    {
        [SerializeField] GameSettingsSetter gameSettingsSetter;
        [SerializeField] TitleScreenUI titleScreenUI;

        void Update()
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.X))
            {
                titleScreenUI.TogglePregameSettingsDebug();
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.M))
            {
                gameSettingsSetter.IncMapIndex();
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.H))
            {
                gameSettingsSetter.IncStartHealths();
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.B))
            {
                gameSettingsSetter.IncBosses();
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.I))
            {
                gameSettingsSetter.IncInventories();
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.S))
            {
                gameSettingsSetter.IncGameSpeed();
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.G))
            {
                gameSettingsSetter.IncStartGold();
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.R))
            {
                gameSettingsSetter.IncBossRounds();
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.D))
            {
                gameSettingsSetter.SetToDefault();
            }
        }
    }
}