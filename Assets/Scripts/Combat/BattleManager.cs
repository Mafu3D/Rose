using System;
using Project.GameTiles;
using Project.UI.BattleUI;
using UnityEditor.Timeline.Actions;
using UnityEngine;

namespace Project.Combat
{
    public class BattleManager
    {
        [field: SerializeField] public float TimeBetweenCombatTurns { get; private set; } = 0.25f;
        [field: SerializeField] public float TimeBeforeCombatStarts { get; private set; } = 1f;
        [field: SerializeField] public float TimeAfterCombatEnds { get; private set; } = 1f;
        public bool DebugMode = true;

        public Battle ActiveBattle;
        public bool IsActiveBattle => ActiveBattle != null;
        public event Action OnNewBattleInitiated;
        public event Action OnBattleConcluded;


        public void StartNewBattle(Character left, Character right, Action<BattleReport, Character, Character> finished)
        {
            ActiveBattle = new Battle(left, right, finished, true);
            OnNewBattleInitiated?.Invoke();
            ActiveBattle.StartPreBattle();
        }

        public void Update()
        {
            if (IsActiveBattle)
            {
                ActiveBattle.Update();
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