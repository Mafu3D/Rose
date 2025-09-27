using Project;
using Project.Combat;
using Project.GameNode;
using Project.GameNode.Hero;
using TMPro;
using UnityEngine;

public class BattleUI : Singleton<BattleUI>
{
    [SerializeField] GameObject MainContainer;

    [SerializeField] CombatantUI LeftCombatantUI;
    [SerializeField] CombatantUI RightCombatantUI;

    [SerializeField] TMP_Text BattleLog;

    Battle activeBattle;

    protected override void Awake()
    {
        base.Awake();
        MainContainer.SetActive(false);
        BattleLog.text = "";
    }

    public void OpenBattleUI(Battle battle, HeroData heroData, NodeData monsterData)
    {
        MainContainer.SetActive(true);

        activeBattle = battle;

        LeftCombatantUI.InitializeCombatant(activeBattle.Hero, heroData.Sprite, heroData.DisplayName, "");
        RightCombatantUI.InitializeCombatant(activeBattle.Monster, monsterData.Sprite, monsterData.DisplayName, monsterData.Description);

        BattleLog.text = "";

        battle.OnBattleAction += UpdateUI;
    }

    public void CloseBattleUI(Battle battle)
    {

        battle.OnBattleAction -= UpdateUI;
        activeBattle = null;
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
