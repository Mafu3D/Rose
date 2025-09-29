using System;
using Project.GameNode;
using Project.UI.BattleUI;
using UnityEditor.Timeline.Actions;
using UnityEngine;

namespace Project.Combat
{
    public class BattleManager : Singleton<BattleManager>
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
            BattleUI.Instance.OpenBattleUI();
            ActiveBattle.InitiateBattle();
            OnNewBattleInitiated?.Invoke();
        }

        // public bool IsActiveBattleConcluded()
        // {
        //     // Could clean this up
        //     // Debug.Log("active: " + IsActiveBattle);
        //     // if (IsActiveBattle)
        //     // {
        //     //     Debug.Log("conclude: " + (ActiveBattle.GetBattleState() == BattleState.Conclude));
        //     //     if (ActiveBattle.GetBattleState() == BattleState.Conclude)
        //     //     {
        //     //         return true;
        //     //     }
        //     //     return false;
        //     // }
        //     // return true;
        //     if (IsActiveBattle)
        //     {
        //         BattleReport battleReport = ActiveBattle.GetLastBattleReport();
        //         if (battleReport.BattleConcluded)
        //         {

        //         }
        //     }
        //     return false;
        // }

        public Status Proceed()
        {
            if (ActiveBattle == null) return Status.Complete;
            ActiveBattle.Proceed();
            BattlePhase battleState = ActiveBattle.GetPhase();
            if (battleState == BattlePhase.Conclude)
            {
                BattleUI.Instance.CloseBattleUI();
                EndActiveBattle();
                return Status.Complete;
            }
            return Status.Running;
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