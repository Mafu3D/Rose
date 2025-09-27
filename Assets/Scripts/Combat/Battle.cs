using System;
using Project.GameNode;
using Project.GameNode.Hero;
using UnityEngine;

namespace Project.Combat
{
    public enum BattleState
    {
        NotStarted,
        RoundStart,
        TurnOne,
        TurnTwo,
        Complete
    }

    public enum BattleResolution
    {
        None,
        LeftSideWon,
        RightSideWon
    }

    public class Battle
    {
        public readonly CombatNode Left;
        public readonly CombatNode Right;
        public int Round;
        public int Turn;
        CombatNode[] combatantOrder = new CombatNode[2];
        BattleState battleState = BattleState.NotStarted;
        BattleResolution battleResolution = BattleResolution.None;

        float timer;

        public event Action<string> OnBattleAction;


        public Battle(CombatNode left, CombatNode right)
        {
            this.Left = left;
            this.Right = right;
        }

        public void StartBattle()
        {
            Round = 0;
            Turn = 0;
            NewRound();
        }

        public BattleResolution Proceed()
        {
            ProcessBattle();
            return battleResolution;
        }

        private BattleState ProcessBattle()
        {
            switch (battleState)
            {
                case BattleState.NotStarted:
                    battleState = BattleState.TurnOne;
                    return battleState;

                case BattleState.TurnOne:
                    IncrementTurn();
                    ProcessAttack(0);

                    battleResolution = CheckForCombatResolution();
                    if (battleResolution != BattleResolution.None)
                    {
                        battleState = BattleState.Complete;
                    }
                    else
                    {
                        battleState = BattleState.TurnTwo;
                    }
                    return battleState;

                case BattleState.TurnTwo:
                    IncrementTurn();
                    ProcessAttack(1);

                    battleResolution = CheckForCombatResolution();
                    if (battleResolution != BattleResolution.None)
                    {
                        battleState = BattleState.Complete;
                    }
                    else
                    {
                        NewRound();
                        battleState = BattleState.TurnOne;
                    }
                    return battleState;
                case BattleState.Complete:
                    return battleState;
            }
            return battleState;
        }

        private void NewRound()
        {
            Round += 1;
            ResetTurns();
            DetermineCombatantOrder();

        }

        private void DetermineCombatantOrder()
        {
            combatantOrder = new CombatNode[2];

            if (Left.Attributes.GetAttributeValue(Attributes.AttributeType.Speed) > Right.Attributes.GetAttributeValue(Attributes.AttributeType.Speed))
            {
                combatantOrder[0] = Left;
                combatantOrder[1] = Right;
            }
            else
            {
                combatantOrder[0] = Right;
                combatantOrder[1] = Left;
            }
        }

        private void IncrementTurn() => Turn += 1;
        private void ResetTurns() => Turn = 0;

        private void ProcessAttack(int i)
        {
            int attackValue;
            combatantOrder[i].Attack(out attackValue);
            HitReport hitReport = new HitReport(attackValue);
            string message;

            if (i == 0)
            {
                combatantOrder[1].ReceiveAttack(hitReport);
                message = $"{combatantOrder[1].NodeData.DisplayName} took {hitReport.Damage} dmg";
            }
            else
            {
                combatantOrder[0].ReceiveAttack(hitReport);
                message = $"{combatantOrder[0].NodeData.DisplayName} took {hitReport.Damage} dmg";
            }

            OnBattleAction?.Invoke(message);

            CheckForCombatResolution();
        }

        private BattleResolution CheckForCombatResolution()
        {
            if (Left.Attributes.GetAttributeValue(Attributes.AttributeType.Health) <= 0)
            {
                battleResolution = BattleResolution.RightSideWon;
            }
            else if (Right.Attributes.GetAttributeValue(Attributes.AttributeType.Health) <= 0)
            {
                battleResolution = BattleResolution.LeftSideWon;
            }
            else
            {
                battleResolution = BattleResolution.None;
            }
            return battleResolution;
        }

        private void FinishBattle()
        {

        }
    }
}