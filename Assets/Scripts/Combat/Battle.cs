using System;
using System.Collections.Generic;
using Project.GameNode;
using Project.GameNode.Hero;
using UnityEngine;

namespace Project.Combat
{
    public struct BattleReport
    {
        public bool BattleConcluded;
        public Resolution Resolution;
        public string Message;

        public BattleReport(bool battleConcluded, Resolution resolution, string message)
        {
            this.BattleConcluded = battleConcluded;
            this.Resolution = resolution;
            this.Message = message;
        }
    }

    public enum Resolution
    {
        None,
        Victory,
        Defeat,
        RanAway,
        Stole
    }

    public enum BattlePhase
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
        public readonly Combatant Hero;
        public readonly Combatant Enemy;
        public int Round;
        public int Turn;
        Combatant[] combatantOrder = new Combatant[2];
        BattlePhase phase = BattlePhase.NotStarted;

        BattleReport latestBattleReport = new BattleReport();

        Action<BattleReport, Combatant, Combatant> finishedCallback;
        public BattleReport GetLatestBattleReport() => latestBattleReport;
        public BattlePhase GetPhase() => phase;

        public Choice<PrebattleActions> PreBattleChoice;

        public event Action<string> OnBattleMessage;
        public event Action<BattlePhase> OnPhaseChanged;
        public event Action OnChooseFight;
        public event Action OnChooseRun;
        public event Action OnChooseSteal;

        private bool ranAway;
        private bool avoidedRunDamage;


        public Battle(Combatant hero, Combatant enemy, Action<BattleReport, Combatant, Combatant> finished)
        {
            this.Hero = hero;
            this.Enemy = enemy;
            finishedCallback = finished;
        }

        public void InitiateBattle()
        {
            phase = BattlePhase.Prebattle;
            List<PrebattleActions> prebattleChoices = new List<PrebattleActions> { PrebattleActions.Fight, PrebattleActions.Steal, PrebattleActions.Run };
            PreBattleChoice = new Choice<PrebattleActions>(prebattleChoices, ResolvePrebattleChoice);
            OnPhaseChanged?.Invoke(phase);

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
            phase = BattlePhase.Steal;
            OnPhaseChanged?.Invoke(phase);
            OnChooseSteal?.Invoke();
        }

        private void ChooseRun()
        {
            Debug.Log("I choose to run!!!");
            RunAway();
        }

        private void RunAway()
        {
            ranAway = true;
            phase = BattlePhase.Run;
            if (CheckIfHeroHasHigherSpeed())
            {
                avoidedRunDamage = true;
                LogBattleAction("avoided damage"); // message isn't actually used
            }
            else
            {
                avoidedRunDamage = false;
                DoAttack(Enemy, Hero);
            }
            OnChooseRun?.Invoke();
            OnPhaseChanged?.Invoke(phase);
        }

        private bool CheckIfHeroHasHigherSpeed()
        {
            int heroSpeed = Hero.Attributes.GetAttributeValue(Attributes.AttributeType.Speed);
            int enemySpeed = Enemy.Attributes.GetAttributeValue(Attributes.AttributeType.Speed);
            if (heroSpeed > enemySpeed)
            {
                return true;
            }
            return false;
        }

        public void Proceed()
        {
            ProcessBattle();
            Debug.Log(phase);
        }

        private BattlePhase ProcessBattle()
        {
            switch (phase)
            {
                case BattlePhase.Prebattle:
                    return phase;

                case BattlePhase.Start:
                    phase = BattlePhase.FirstTurn;
                    OnPhaseChanged?.Invoke(phase);
                    return phase;

                case BattlePhase.FirstTurn:
                    IncrementTurn();
                    DoAttackForTurn(0);

                    if (latestBattleReport.BattleConcluded)
                    {
                        phase = BattlePhase.PostBattle;
                        OnPhaseChanged?.Invoke(phase);
                    }
                    else
                    {
                        phase = BattlePhase.SecondTurn;
                        OnPhaseChanged?.Invoke(phase);
                    }
                    return phase;

                case BattlePhase.SecondTurn:
                    IncrementTurn();
                    DoAttackForTurn(1);

                    if (latestBattleReport.BattleConcluded)
                    {
                        phase = BattlePhase.PostBattle;
                        OnPhaseChanged?.Invoke(phase);
                    }
                    else
                    {
                        NewRound();
                        phase = BattlePhase.FirstTurn;
                        OnPhaseChanged?.Invoke(phase);
                    }
                    return phase;

                case BattlePhase.Run:
                    Debug.Log("You ran away!");
                    phase = BattlePhase.PostBattle;
                    OnPhaseChanged?.Invoke(phase);
                    return phase;

                case BattlePhase.Steal:
                    Debug.Log("You got some gold!");
                    phase = BattlePhase.PostBattle;
                    OnPhaseChanged?.Invoke(phase);
                    return phase;

                case BattlePhase.PostBattle:
                    phase = BattlePhase.Conclude;
                    ConcludeBattle();
                    OnPhaseChanged?.Invoke(phase);
                    return phase;

                case BattlePhase.Conclude:

                    return phase;
            }
            return phase;
        }

        private void StartBattle()
        {
            phase = BattlePhase.Start;
            OnPhaseChanged?.Invoke(phase);
            Round = 0;
            Turn = 0;
            OnChooseFight?.Invoke();
            OnPhaseChanged?.Invoke(phase);
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

            if (Hero.Attributes.GetAttributeValue(Attributes.AttributeType.Speed) > Enemy.Attributes.GetAttributeValue(Attributes.AttributeType.Speed))
            {
                combatantOrder[0] = Hero;
                combatantOrder[1] = Enemy;
            }
            else
            {
                combatantOrder[0] = Enemy;
                combatantOrder[1] = Hero;
            }
        }

        private void IncrementTurn() => Turn += 1;
        private void ResetTurns() => Turn = 0;

        private void DoAttackForTurn(int turn)
        {
            if (turn == 0) // First turn
            {
                DoAttack(combatantOrder[0], combatantOrder[1]);
            }
            else // Second turn
            {
                DoAttack(combatantOrder[1], combatantOrder[0]);
            }
        }

        private void DoAttack(Combatant attacker, Combatant defender) {
            int attackValue;
            attacker.Attack(out attackValue);
            HitReport hitReport = new HitReport(attackValue);
            defender.ReceiveAttack(hitReport);
            LogBattleAction($"{defender.DisplayName} took {hitReport.Damage} dmg");
        }

        private void LogBattleAction(string message)
        {
            OnBattleMessage?.Invoke(message);
            CreateBattleReport();
        }

        private void CreateBattleReport()
        {
            bool battleConcluded;
            Resolution resolution;
            string message;

            if (ranAway)
            {
                battleConcluded = true;
                resolution = Resolution.RanAway;
                if (avoidedRunDamage)
                {
                    message = "You ran away without taking damage!";
                }
                else
                {
                    message = "You ran away but took some damage!";
                }
            }
            else
            {
                if (Hero.Attributes.GetAttributeValue(Attributes.AttributeType.Health) <= 0)
                {
                    battleConcluded = true;
                    resolution = Resolution.Defeat;
                    // message = $"{Enemy.DisplayName} has defeated {Hero.DisplayName}";
                    message = $"{Hero.DisplayName} was slain by a {Enemy.DisplayName}";
                }
                else if (Enemy.Attributes.GetAttributeValue(Attributes.AttributeType.Health) <= 0)
                {
                    battleConcluded = true;
                    resolution = Resolution.Victory;
                    // message = $"{Hero.DisplayName} has defeated {Enemy.DisplayName}";
                    message = $"{Hero.DisplayName} has defeated the {Enemy.DisplayName}";
                }
                else
                {
                    battleConcluded = false;
                    resolution = Resolution.None;
                    message = $"The battle rages on!";
                }
            }

            latestBattleReport = new BattleReport(battleConcluded, resolution, message);
        }

        private void ConcludeBattle()
        {
            finishedCallback(latestBattleReport, Hero, Enemy);
        }
    }
}