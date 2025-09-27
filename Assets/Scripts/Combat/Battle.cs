using System;
using Project.GameNode;
using Project.GameNode.Hero;
using UnityEngine;

namespace Project.Combat
{
    public enum BattleState
    {
        NotStarted,
        TurnOne,
        TurnOneBuffer,
        TurnTwo,
        TurnTwoBuffer,
        Victory,
        Defeat
    }

    public enum CombatResolution
    {
        None,
        LeftSideWon,
        RightSideWon
    }

    public class Battle
    {
        public readonly ICombatantNode Hero;
        public readonly ICombatantNode Monster;
        public int Round;
        public int Turn;
        ICombatantNode[] combatantOrder = new ICombatantNode[2];
        BattleState battleState = BattleState.NotStarted;

        float timer;

        public event Action<string> OnBattleAction;


        public Battle(ICombatantNode hero, ICombatantNode monster)
        {
            this.Hero = hero;
            this.Monster = monster;
        }

        public void StartBattle()
        {
            Round = 0;
            Turn = 0;
            NewRound();
        }

        public Status ProcessBattle()
        {
            switch (battleState)
            {
                case BattleState.NotStarted:
                    return Status.Running;

                case BattleState.TurnOne:
                    timer = 0f;
                    battleState = BattleState.TurnOneBuffer;
                    NextTurn();
                    return Status.Running;

                case BattleState.TurnOneBuffer:
                    timer += Time.deltaTime;
                    if (timer > GameManager.Instance.TimeBetweenCombatTurns)
                    {
                        timer = 0f;
                        battleState = BattleState.TurnTwo;
                    }
                    return Status.Running;

                case BattleState.TurnTwo:
                    timer = 0f;
                    battleState = BattleState.TurnTwoBuffer;
                    NextTurn();
                    return Status.Running;

                case BattleState.TurnTwoBuffer:
                    timer += Time.deltaTime;
                    if (timer > GameManager.Instance.TimeBetweenCombatTurns)
                    {
                        timer = 0f;
                        battleState = BattleState.TurnOne;
                        NewRound();
                    }
                    return Status.Running;

                case BattleState.Victory:
                    FinishBattle();
                    return Status.Success;

                case BattleState.Defeat:
                    FinishBattle();
                    return Status.Success;
            }

            FinishBattle();
            return Status.Success;
        }

        private void NewRound()
        {
            Round += 1;
            Turn = 0;
            combatantOrder = new ICombatantNode[2];
            battleState = BattleState.TurnOne;

            // Determine round order
            if (Hero.GetSpeedValue() > Monster.GetSpeedValue())
            {
                combatantOrder[0] = Hero;
                combatantOrder[1] = Monster;
            }
            else
            {
                combatantOrder[0] = Monster;
                combatantOrder[1] = Hero;
            }
        }

        private void NextTurn()
        {
            Turn += 1;
            ProcessAttack(Turn - 1);

        }

        private void ProcessAttack(int i)
        {
            int attackValue;
            combatantOrder[i].Attack(out attackValue);
            HitReport hitReport = new HitReport(attackValue);
            string message = "";

            if (i == 0)
            {
                combatantOrder[i + 1].ReceiveAttack(hitReport);
                message = $"Hero took {hitReport.Damage} dmg";
            }
            else
            {
                combatantOrder[i - 1].ReceiveAttack(hitReport);
                message = $"Enemy took {hitReport.Damage} dmg";
            }

            OnBattleAction?.Invoke(message);

            CheckForCombatResolution();
        }

        private void CheckForCombatResolution()
        {
            if (Hero.GetHealthValue() <= 0)
            {
                battleState = BattleState.Defeat;
            }

            if (Monster.GetHealthValue() <= 0)
            {
                battleState = BattleState.Victory;
            }
        }

        private void FinishBattle()
        {

        }
    }
}