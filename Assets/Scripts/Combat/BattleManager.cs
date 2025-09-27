using System;
using Project.GameNode;
using Project.UI.BattleUI;
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


        public void StartNewBattle(CombatNode left, CombatNode right)
        {
            ActiveBattle = new Battle(left, right);
            ActiveBattle.StartBattle();
            OnBattleStart?.Invoke();
            BattleUI.Instance.OpenBattleUI();
        }

        public Status Proceed()
        {
            if (ActiveBattle == null) return Status.Failure;

            switch (ActiveBattle.Proceed())
            {
                case BattleResolution.None:
                    return Status.Running;
                default:
                    BattleUI.Instance.CloseBattleUI();
                    EndActiveBattle();
                    return Status.Success;
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