using System;
using Project.GameTiles;
using Project.UI.BattleUI;
using UnityEngine;

namespace Project.Combat
{
    public class BattleManager
    {
        public bool DebugMode = true;

        public Battle ActiveBattle;
        public bool IsActiveBattle => ActiveBattle != null;
        public event Action OnNewBattleInitiated;
        public event Action OnBattleConcluded;
        public event Action AutoTimerTick;
        private float autoTimer;


        public void StartNewBattle(Character left, Character right, Action<BattleReport, Character, Character> finished)
        {
            ActiveBattle = new Battle(left, right, finished, true);
            OnNewBattleInitiated?.Invoke();
            ActiveBattle.InitializeFight();
        }

        public void Update()
        {
            if (IsActiveBattle)
            {
                ActiveBattle.Update();
            }

            if (GameManager.Instance.AutoBattle)
            {
                autoTimer += Time.deltaTime;
                if (autoTimer > GameManager.Instance.AutoBattleSpeed)
                {
                    autoTimer = 0f;
                    AutoTimerTick?.Invoke();
                }
            }
        }

        public void EndActiveBattle()
        {
            OnBattleConcluded?.Invoke();
            if (ActiveBattle != null)
            {
                ActiveBattle = null;
            }
        }
    }
}