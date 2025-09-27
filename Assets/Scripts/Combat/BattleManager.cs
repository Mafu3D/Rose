using Project.GameNode;
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


        public void StartNewBattle(CombatNode left, CombatNode right)
        {
            ActiveBattle = new Battle(left, right);
            ActiveBattle.StartBattle();
            BattleUI.Instance.OpenBattleUI();
        }

        public Status Process()
        {
            if (ActiveBattle == null) return Status.Failure;

            switch (ActiveBattle.ProcessBattle())
            {
                case Status.Running:
                    break;
                case Status.Success:
                    BattleUI.Instance.CloseBattleUI();
                    EndActiveBattle();
                    return Status.Success;
                default:
                    return Status.Failure;
            }
            return Status.Running;
        }

        public void EndActiveBattle()
        {
            if (ActiveBattle != null)
            {
                ActiveBattle = null;
            }
        }
    }
}