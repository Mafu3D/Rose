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
        [SerializeField] GameObject MainContainer;

        [SerializeField] CombatantUI LeftCombatantUI;
        [SerializeField] CombatantUI RightCombatantUI;

        [SerializeField] TMP_Text BattleLog;

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

            activeBattle.OnBattleAction += UpdateUI;
        }

        public void CloseBattleUI()
        {

            activeBattle.OnBattleAction -= UpdateUI;
            MainContainer.SetActive(false);
        }

        void UpdateUI(string message)
        {
            // int lines = BattleLog.text.Split('\n').Length;
            LeftCombatantUI.UpdateStats();
            RightCombatantUI.UpdateStats();
            BattleLog.text += $"{message} \n";
        }
    }
}