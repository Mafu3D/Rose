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
        public event Action OnBattleStart;
        public event Action OnBattleEnd;


        public void StartNewBattle(Combatant left, Combatant right, Action<BattleResolution, Combatant, Combatant> finished)
        {
            ActiveBattle = new Battle(left, right, finished);
            ActiveBattle.StartBattle();
            OnBattleStart?.Invoke();
            BattleUI.Instance.OpenBattleUI();
        }

        public BattleResolution GetActiveBattleResolution()
        {
            if (IsActiveBattle)
            {
                return ActiveBattle.GetBattleResolution();
            }
            return BattleResolution.None;
        }

        public Status Proceed()
        {
            if (ActiveBattle == null) return Status.Complete;

            switch (ActiveBattle.Proceed())
            {
                case BattleResolution.None:
                    return Status.Running;
                default:
                    BattleUI.Instance.CloseBattleUI();
                    EndActiveBattle();
                    return Status.Complete;
            }
        }

        public void EndActiveBattle()
        {
            if (ActiveBattle != null)
            {
                ActiveBattle = null;
            }
            OnBattleEnd?.Invoke();
        }
    }
}