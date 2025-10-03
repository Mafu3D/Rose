using System;
using Project.GameNode;
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

        public Battle ActiveBattle;
        public bool IsActiveBattle => ActiveBattle != null;
        public event Action OnNewBattleInitiated;
        public event Action OnBattleConcluded;


        public void StartNewBattle(Combatant left, Combatant right, Action<BattleReport, Combatant, Combatant> finished)
        {
            ActiveBattle = new Battle(left, right, finished);
            OnNewBattleInitiated?.Invoke();
            ActiveBattle.InitiateBattle();
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
            if (ActiveBattle != null)
            {
                ActiveBattle = null;
            }
            OnBattleConcluded?.Invoke();
        }
    }
}