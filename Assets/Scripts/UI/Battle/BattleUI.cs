using System;
using System.Collections.Generic;
using Project.Combat;
using TMPro;
using Unity.Mathematics;
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
        [SerializeField] int MaxLogLength = 6;

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

            activeBattle.OnBattleInitialize += ShowPreBattleUI;
            activeBattle.OnBattleStart += ShowActiveBattleUI;
            activeBattle.OnBattleDecided += ShowPostBattleUI;
            activeBattle.OnBattleLogUpdated += UpdateBattleLog;
            activeBattle.OnNewTurn += UpdateActiveCombatant;
            activeBattle.OnAttack += PlayAttackVFX;
            // activeBattle.OnChooseRun += UpdateBattleUI;
            // activeBattle.OnChooseSteal += UpdateBattleUI;
        }

        private void ShowPostBattleUI(BattleReport battleReport)
        {
            LeftCombatantUI.SetActiveCombatant(false);
            RightCombatantUI.SetActiveCombatant(false);

            PrebattleContainer.SetActive(false);
            ActiveBattleContainer.SetActive(false);
            PostbattleContainer.SetActive(true);

            string title;
            switch (battleReport.Resolution)
            {
                case Combat.Resolution.RanAway:
                    title = "Ran Away!";
                    break;
                case Combat.Resolution.Stole:
                    title = "You Stole!";
                    break;
                case Combat.Resolution.Defeat:
                    title = "You were defeated!";
                    break;
                case Combat.Resolution.Victory:
                    title = "You won!";
                    break;
                default:
                    title = "Something went wrong";
                    break;
            }
            ResultTitleText.text = title;
            ResultMessageText.text = battleReport.Message;
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
            activeBattle.OnBattleInitialize -= ShowPreBattleUI;
            activeBattle.OnBattleStart -= ShowActiveBattleUI;
            activeBattle.OnBattleDecided -= ShowPostBattleUI;
            activeBattle.OnBattleLogUpdated -= UpdateBattleLog;
            activeBattle.OnNewTurn -= UpdateActiveCombatant;
            activeBattle.OnAttack -= PlayAttackVFX;
            // activeBattle.OnChooseRun -= UpdateBattleUI;
            // activeBattle.OnChooseSteal -= UpdateBattleUI;

            MainContainer.SetActive(false);
        }

        private void UpdateActiveCombatant()
        {
            int activeCombatantTEMP = activeBattle.GetWhichSideIsActiveTEMP();
            if (activeCombatantTEMP == 0)
            {
                LeftCombatantUI.SetActiveCombatant(true);
                RightCombatantUI.SetActiveCombatant(false);
            }
            else if (activeCombatantTEMP == 1)
            {
                LeftCombatantUI.SetActiveCombatant(false);
                RightCombatantUI.SetActiveCombatant(true);
            }
            else
            {
                LeftCombatantUI.SetActiveCombatant(false);
                RightCombatantUI.SetActiveCombatant(false);
            }
        }

        private void PlayAttackVFX()
        {
            int activeCombatantTEMP = activeBattle.GetWhichSideIsActiveTEMP();
            if (activeCombatantTEMP == 0)
            {
                LeftCombatantUI.PlayAttackVFX();
            }
            else if (activeCombatantTEMP == 1)
            {
                RightCombatantUI.PlayAttackVFX();
            }
        }

        void UpdateBattleLog(string battleLog)
        {
            string[] lines = battleLog.Split('\n');
            string truncatedLog = "";

            int startingI = Math.Clamp(lines.Length - MaxLogLength, 0, 999999);
            for (int i = startingI; i < lines.Length; i++) truncatedLog += $"{lines[i]} \n";
            LeftCombatantUI.UpdateStats();
            LeftCombatantUI.UpdateStatusEffects();
            RightCombatantUI.UpdateStats();
            RightCombatantUI.UpdateStatusEffects();
            BattleLog.text = truncatedLog;
        }
    }
}