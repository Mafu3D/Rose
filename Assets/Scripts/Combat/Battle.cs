using System;
using System.Collections.Generic;
using Project.GameNode;
using Project.GameNode.Hero;
using UnityEngine;

namespace Project.Combat
{
    public struct BattleReport
    {
        public bool BattleDecided;
        public int WinnerIndex;
        public string Message;
        public BattleReport(bool battleDecided, int winnerIndex, string message)
        {
            this.BattleDecided = battleDecided;
            this.WinnerIndex = winnerIndex;
            this.Message = message;
        }
    }

    public enum BattleState
    {
        NotStarted,
        Prebattle,
        Run,
        Steal,
        Start,
        FirstTurn,
        SecondTurn,
        PostBattle,
        Conclude
    }

    public enum PrebattleActions
    {
        Fight,
        Steal,
        Run
    }

    public class Battle
    {
        public readonly Combatant Left;
        public readonly Combatant Right;
        public int Round;
        public int Turn;
        Combatant[] combatantOrder = new Combatant[2];
        BattleState battleState = BattleState.NotStarted;

        BattleReport lastBattleReport = new BattleReport();

        Action<BattleReport, Combatant, Combatant> finishedCallback;
        public BattleReport GetLastBattleReport() => lastBattleReport;
        public BattleState GetBattleState() => battleState;

        public Choice<PrebattleActions> PreBattleChoice;

        public event Action<string> OnBattleMessage;
        public event Action OnBattleInitiated;
        public event Action OnBattleStart;
        public event Action OnBattleDecided;
        public event Action OnBattleConclude;
        public event Action OnChooseRun;
        public event Action OnChooseSteal;


        public Battle(Combatant left, Combatant right, Action<BattleReport, Combatant, Combatant> finished)
        {
            this.Left = left;
            this.Right = right;
            finishedCallback = finished;
        }

        public void InitiateBattle()
        {
            battleState = BattleState.Prebattle;
            List<PrebattleActions> prebattleChoices = new List<PrebattleActions> { PrebattleActions.Fight, PrebattleActions.Steal, PrebattleActions.Run };
            PreBattleChoice = new Choice<PrebattleActions>(prebattleChoices, ResolvePrebattleChoice);
            OnBattleInitiated?.Invoke();

        }

        private void ResolvePrebattleChoice(PrebattleActions actionChoice)
        {
            switch (actionChoice)
            {
                case PrebattleActions.Fight:
                    ChooseFight();
                    break;
                case PrebattleActions.Steal:
                    ChooseSteal();
                    break;
                case PrebattleActions.Run:
                    ChooseRun();
                    break;
            }
            PreBattleChoice = null;
        }

        private void ChooseFight()
        {
            Debug.Log("I choose to fight!");
            StartBattle();
        }

        private void ChooseSteal()
        {
            Debug.Log("I choose to steal!");
            battleState = BattleState.Steal;
            OnChooseSteal?.Invoke();
        }

        private void ChooseRun()
        {
            Debug.Log("I choose to run!!!");
            battleState = BattleState.Run;
            OnChooseRun?.Invoke();
        }

        public void Proceed()
        {
            ProcessBattle();
            Debug.Log(battleState);
        }

        private BattleState ProcessBattle()
        {
            switch (battleState)
            {
                case BattleState.Prebattle:
                    return battleState;

                case BattleState.Start:
                    battleState = BattleState.FirstTurn;
                    return battleState;

                case BattleState.FirstTurn:
                    IncrementTurn();
                    ProcessAttack(0);


                    CreateBattleReport();
                    if (lastBattleReport.BattleDecided)
                    {
                        battleState = BattleState.PostBattle;
                        OnBattleDecided?.Invoke();
                    }
                    else
                    {
                        battleState = BattleState.SecondTurn;
                    }
                    return battleState;

                case BattleState.SecondTurn:
                    IncrementTurn();
                    ProcessAttack(1);

                    if (lastBattleReport.BattleDecided)
                    {
                        battleState = BattleState.PostBattle;
                        OnBattleDecided?.Invoke();
                    }
                    else
                    {
                        NewRound();
                        battleState = BattleState.FirstTurn;
                    }
                    return battleState;

                case BattleState.Run:
                    Debug.Log("You ran away!");
                    battleState = BattleState.Conclude;
                    return battleState;

                case BattleState.Steal:
                    Debug.Log("You got some gold!");
                    battleState = BattleState.Conclude;
                    return battleState;

                case BattleState.PostBattle:
                    battleState = BattleState.Conclude;
                    return battleState;

                case BattleState.Conclude:
                    ConcludeBattle();
                    return battleState;
            }
            return battleState;
        }

        private void StartBattle()
        {
            battleState = BattleState.Start;
            Round = 0;
            Turn = 0;
            OnBattleStart?.Invoke();
            NewRound();
        }

        private void NewRound()
        {
            Round += 1;
            ResetTurns();
            DetermineCombatantOrder();
        }

        private void DetermineCombatantOrder()
        {
            combatantOrder = new Combatant[2];

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
                message = $"{combatantOrder[1].DisplayName} took {hitReport.Damage} dmg";
            }
            else
            {
                combatantOrder[0].ReceiveAttack(hitReport);
                message = $"{combatantOrder[0].DisplayName} took {hitReport.Damage} dmg";
            }

            OnBattleMessage?.Invoke(message);
        }

        private void CreateBattleReport()
        {
            bool battleDecided;
            int winnerIndex;
            string message;

            if (Left.Attributes.GetAttributeValue(Attributes.AttributeType.Health) <= 0)
            {
                battleDecided = true;
                winnerIndex = 1;
                message = $"{Right.DisplayName} has defeated {Left.DisplayName}";
            }
            else if (Right.Attributes.GetAttributeValue(Attributes.AttributeType.Health) <= 0)
            {
                battleDecided = true;
                winnerIndex = 0;
                message = $"{Left.DisplayName} has defeated {Right.DisplayName}";
            }
            else
            {
                battleDecided = false;
                winnerIndex = default;
                message = $"The battle was not decided";
            }

            lastBattleReport = new BattleReport(battleDecided, winnerIndex, message);
        }

        private void ConcludeBattle()
        {
            finishedCallback(lastBattleReport, Left, Right);
        }
    }
}