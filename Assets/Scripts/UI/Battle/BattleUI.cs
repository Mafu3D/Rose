using System;
using System.Collections.Generic;
using Project.Combat;
using Project.Combat.CombatStates;
using Project.Items;
using Project.VFX;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.BattleUI
{
    public class BattleUI : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;

        [Header("Main")]
        [SerializeField] GameObject MainContainer;
        [SerializeField] UIShaker MainUIShaker;

        [Header("Combatants")]
        [SerializeField] CombatantUI LeftCombatantUI;
        [SerializeField] CombatantUI RightCombatantUI;

        [Header("Prebattle")]
        [SerializeField] GameObject PrebattleContainer;
        [SerializeField] Button fightButton;
        [SerializeField] Button runButton;
        [SerializeField] List<Image> consumableItemSlots;
        [SerializeField] Sprite unequippedSprite;

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
            activeBattle.Hero.OnReceiveHit += ShakeUIHero;
            activeBattle.Enemy.OnReceiveHit += ShakeUIEnemy;
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

            UpdateConsumableInventoryUI();
        }

        public void UseConsumableItem(int index)
        {
            GameManager.Instance.Player.HeroTile.Character.Inventory.UseConsumableItemPrecombat(index);
            UpdateConsumableInventoryUI();
        }

        private void UpdateConsumableInventoryUI()
        {
            List<Item> consumableItems = GameManager.Instance.Player.HeroTile.Character.Inventory.GetConsumableItems();
            for (int i = 0; i < consumableItemSlots.Count; i++)
            {
                if (consumableItems.Count > i)
                {
                    consumableItemSlots[i].sprite = consumableItems[i].ItemData.Sprite;
                    if (consumableItems[i].ItemData.OnOverworldUse.Count == 0)
                    {
                        Color color = consumableItemSlots[i].color;
                        color.a = 0.25f;
                        consumableItemSlots[i].color = color;
                    }
                    else
                    {
                        Color color = consumableItemSlots[i].color;
                        color.a = 1;
                        consumableItemSlots[i].color = color;
                    }
                }
                else
                {
                    consumableItemSlots[i].sprite = unequippedSprite;
                    Color color = consumableItemSlots[i].color;
                    color.a = 1;
                    consumableItemSlots[i].color = color;
                }
            }
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
            activeBattle.Hero.OnReceiveHit -= ShakeUIHero;
            activeBattle.Enemy.OnReceiveHit -= ShakeUIEnemy;

            MainContainer.SetActive(false);
        }

        private void ShakeUIEnemy(HitReport report)
        {
            // MainUIShaker.StartShake(0.25f, report.Damage * 2f, 5f);
        }

        private void ShakeUIHero(HitReport report)
        {
            MainUIShaker.StartShake(0.25f, report.Damage * 2.5f, 10f);
        }

        public void PlayEffectVFX(GameObject vfxPrefab, Character target)
        {
            if (target == activeBattle.Hero)
            {
                LeftCombatantUI.PlayEffectVFX(vfxPrefab);
            }
            else if (target == activeBattle.Enemy)
            {
                RightCombatantUI.PlayEffectVFX(vfxPrefab);
            }
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

        public void FightOnClick()
        {
            Battle activeBattle = GameManager.Instance.BattleManager.ActiveBattle;
            activeBattle.PreBattleChoice.ChooseItem(0);
            activeBattle.PreBattleChoice.Resolve();
        }

        public void RunOnClick()
        {
            Battle activeBattle = GameManager.Instance.BattleManager.ActiveBattle;
            activeBattle.PreBattleChoice.ChooseItem(2);
            activeBattle.PreBattleChoice.Resolve();
        }

        public void ContinueOnClick()
        {
            GameManager.Instance.BattleManager.ActiveBattle.StateMachine.SwitchState(new BattleFinishedState("Battle Finished", GameManager.Instance.BattleManager.ActiveBattle.StateMachine, GameManager.Instance));
            // StateMachine.SwitchState(new BattleFinishedState("Battle Finished", StateMachine, GameManager));
        }
    }
}