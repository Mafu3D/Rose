using Project;
using Project.Combat;
using Project.GameNode;
using Project.GameNode.Hero;
using TMPro;
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

            LeftCombatantUI.InitializeCombatant(activeBattle.Left);
            RightCombatantUI.InitializeCombatant(activeBattle.Right);

            BattleLog.text = "";

            activeBattle.OnBattleMessage += UpdateBattleLog;
            activeBattle.OnBattleInitiated += UpdateBattleUI;
            activeBattle.OnBattleStart += UpdateBattleUI;
            activeBattle.OnBattleDecided += UpdateBattleUI;
            activeBattle.OnBattleConclude += UpdateBattleUI;
            activeBattle.OnChooseRun += UpdateBattleUI;
            activeBattle.OnChooseSteal += UpdateBattleUI;
        }

        public void UpdateBattleUI()
        {
            BattleState battleState = BattleManager.Instance.ActiveBattle.GetBattleState();
            switch (battleState)
            {
                case BattleState.Prebattle:
                    ShowPreBattleUI();
                    break;

                case BattleState.Start:
                case BattleState.FirstTurn:
                case BattleState.SecondTurn:
                    ShowActiveBattleUI();
                    break;

                case BattleState.PostBattle:
                    ShowPostBattleUI();
                    break;
            }
        }

        private void ShowPostBattleUI()
        {
            PrebattleContainer.SetActive(false);
            ActiveBattleContainer.SetActive(false);
            PostbattleContainer.SetActive(true);

            if (BattleManager.Instance.ActiveBattle.GetLastBattleReport().WinnerIndex == 0)
            {
                ResultTitleText.text = "Victory";
            }
            else
            {
                ResultTitleText.text = "Defeat";
            }
            ResultMessageText.text = BattleManager.Instance.ActiveBattle.GetLastBattleReport().Message;
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
            activeBattle.OnBattleInitiated -= UpdateBattleUI;
            activeBattle.OnBattleStart -= UpdateBattleUI;
            activeBattle.OnBattleDecided -= UpdateBattleUI;
            activeBattle.OnBattleConclude -= UpdateBattleUI;
            activeBattle.OnChooseRun -= UpdateBattleUI;
            activeBattle.OnChooseSteal -= UpdateBattleUI;

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