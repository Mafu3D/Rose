using Project.Combat;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Project.UI.BattleUI
{
    public class BattleUI : Singleton<BattleUI>
    {
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

        Battle activeBattle => BattleManager.Instance.ActiveBattle;

        protected override void Awake()
        {
            base.Awake();
            MainContainer.SetActive(false);
            BattleLog.text = "";
        }

        public void OpenBattleUI()
        {
            MainContainer.SetActive(true);

            LeftCombatantUI.InitializeCombatant(activeBattle.Hero);
            RightCombatantUI.InitializeCombatant(activeBattle.Enemy);

            BattleLog.text = "";

            activeBattle.OnBattleMessage += UpdateBattleLog;
            activeBattle.OnPhaseChanged += UpdateBattleUI;
            // activeBattle.OnChooseRun += UpdateBattleUI;
            // activeBattle.OnChooseSteal += UpdateBattleUI;
        }

        public void UpdateBattleUI(BattlePhase phase)
        {
            switch (phase)
            {
                case BattlePhase.Prebattle:
                    ShowPreBattleUI();
                    break;

                case BattlePhase.Start:
                // case BattlePhase.FirstTurn:
                // case BattlePhase.SecondTurn:
                    ShowActiveBattleUI();
                    break;

                case BattlePhase.PostBattle:
                    ShowPostBattleUI();
                    break;
            }
        }

        private void ShowPostBattleUI()
        {
            PrebattleContainer.SetActive(false);
            ActiveBattleContainer.SetActive(false);
            PostbattleContainer.SetActive(true);

            switch (BattleManager.Instance.ActiveBattle.GetLatestBattleReport().Resolution)
            {
                case Combat.Resolution.Victory:
                    ResultTitleText.text = "Victory";
                    break;
                case Combat.Resolution.Defeat:
                    ResultTitleText.text = "Defeat";
                    break;
                case Combat.Resolution.RanAway:
                    ResultTitleText.text = "Ran Away";
                    break;
                case Combat.Resolution.Stole:
                    ResultTitleText.text = "You Stole!";
                    break;
                default:
                    ResultTitleText.text = "No Resolution";
                    break;
            }
            ResultMessageText.text = BattleManager.Instance.ActiveBattle.GetLatestBattleReport().Message;
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

        public void CloseBattleUI()
        {

            activeBattle.OnBattleMessage -= UpdateBattleLog;
            activeBattle.OnPhaseChanged -= UpdateBattleUI;
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