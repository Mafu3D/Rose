using System;
using Project.Combat;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Project.UI.BattleUI
{
    public class BattleUI : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;

        [Header("Main")]
        [SerializeField] GameObject MainContainer;

        [Header("Combatants")]
        [SerializeField] CombatantUI LeftCombatantUI;
        [SerializeField] CombatantUI RightCombatantUI;

        [Header("Prebattle")]
        [SerializeField] GameObject PrebattleContainer;

        [Header("During Battle")]
        [SerializeField] GameObject ActiveBattleContainer;
        [SerializeField] TMP_Text BattleLog;

        [Header("Postbattle")]
        [SerializeField] GameObject PostbattleContainer;
        [SerializeField] TMP_Text ResultTitleText;
        [SerializeField] TMP_Text ResultMessageText;

        [Header("Debug")]
        [SerializeField] bool debugState = true;
        [SerializeField] TMP_Text stateDebugText;


        Battle activeBattle => gameManager.BattleManager.ActiveBattle;

        void Awake()
        {
            gameManager.OnGameStartEvent += Initialize;
            MainContainer.SetActive(false);
            BattleLog.text = "";
        }

        void Update()
        {
            // TEMP: REMOVE!
            if (debugState && gameManager.BattleManager.IsActiveBattle)
            {
                stateDebugText.gameObject.SetActive(true);
                stateDebugText.text = gameManager.BattleManager.ActiveBattle.StateMachine.CurrentState.Name;
            }
            else
            {
                stateDebugText.gameObject.SetActive(false);
            }
        }

        private void Initialize()
        {
            gameManager.BattleManager.OnNewBattleInitiated += OpenBattleUI;
            gameManager.BattleManager.OnBattleConcluded += CloseBattleUI;
        }

        private void OpenBattleUI()
        {
            MainContainer.SetActive(true);

            LeftCombatantUI.InitializeCombatant(activeBattle.Hero);
            RightCombatantUI.InitializeCombatant(activeBattle.Enemy);

            BattleLog.text = "";

            activeBattle.OnPreBattleStart += ShowPreBattleUI;
            activeBattle.OnBattleStart += ShowActiveBattleUI;
            activeBattle.OnPostBattleStart += ShowPostBattleUI;
            activeBattle.OnBattleMessage += UpdateBattleLog;
            // activeBattle.OnChooseRun += UpdateBattleUI;
            // activeBattle.OnChooseSteal += UpdateBattleUI;
        }

        private void ShowPostBattleUI()
        {
            PrebattleContainer.SetActive(false);
            ActiveBattleContainer.SetActive(false);
            PostbattleContainer.SetActive(true);

            ResultTitleText.text = "Fix Me";
        }

        private void ShowActiveBattleUI()
        {
            PrebattleContainer.SetActive(false);
            ActiveBattleContainer.SetActive(true);
            PostbattleContainer.SetActive(false);
        }

        private void ShowPreBattleUI()
        {
            PrebattleContainer.SetActive(true);
            ActiveBattleContainer.SetActive(false);
            PostbattleContainer.SetActive(false);
        }

        private void CloseBattleUI()
        {
            activeBattle.OnPreBattleStart -= ShowPreBattleUI;
            activeBattle.OnBattleStart -= ShowActiveBattleUI;
            activeBattle.OnPostBattleStart -= ShowPostBattleUI;
            activeBattle.OnBattleMessage -= UpdateBattleLog;
            // activeBattle.OnChooseRun -= UpdateBattleUI;
            // activeBattle.OnChooseSteal -= UpdateBattleUI;

            MainContainer.SetActive(false);
        }

        void UpdateBattleLog(string message)
        {
            // int lines = BattleLog.text.Split('\n').Length;
            LeftCombatantUI.UpdateStats();
            RightCombatantUI.UpdateStats();
            BattleLog.text += $"{message} \n";
        }
    }
}