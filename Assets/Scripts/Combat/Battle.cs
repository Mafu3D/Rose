using System;
using System.Collections.Generic;
using Project.GameNode;
using Project.GameNode.Hero;
using Project.Items;
using UnityEngine;
using Project.GameLoop;
using Project.Combat.CombatStates;
using Project.Combat.CombatActions;

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
        Combatant activeCombatanat;

        Action<BattleReport, Combatant, Combatant> finishedCallback;

        public Choice<PrebattleActions> PreBattleChoice;

        public event Action<string> OnBattleMessage;

        public event Action OnPreBattleStart;
        public event Action OnBattleStart;
        public event Action OnPostBattleStart;
        public event Action OnChooseFight;
        public event Action OnChooseRun;
        public event Action OnChooseSteal;
        public event Action OnAttackStart;
        public event Action OnAttackEnd;
        public event Action<BattleReport> OnBattleHasBeenDecided;
        public event Action OnNextActionEvent;

        private bool ranAway;
        private bool avoidedRunDamage;

        public StateMachine StateMachine {get; private set; }
        public CombatQueue CombatQueue {get; private set; }


        public Battle(Combatant hero, Combatant enemy, Action<BattleReport, Combatant, Combatant> finished)
        {
            this.Hero = hero;
            this.Enemy = enemy;
            finishedCallback = finished;

            // State Machine
            StateMachine = new StateMachine();
            CombatQueue = new CombatQueue();
        }

        #region Initiate

        public void InitiateBattle()
        {
            StateMachine.SetInitialState(new PreBattleState("Pre Battle", StateMachine, GameManager.Instance));
            OnPreBattleStart?.Invoke();
        }

        #endregion

        public void Update()
        {
            CombatQueue.Update();
            StateMachine.Update();
        }

        #region Prebattle

        public void ChooseFight()
        {
            // StartBattle();
        }

        public void ChooseSteal()
        {
            OnChooseSteal?.Invoke();
        }

        public void ChooseRun()
        {
            RunAway();
        }

        #endregion

        #region Run

        private void RunAway()
        {
            ranAway = true;
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

        #endregion

        #region Steal

        #endregion

        #region Battle Loop

        public void NextAction()
        {
            OnNextActionEvent?.Invoke();
            CombatQueue.TriggerNextAction();
        }

        public void DetermineCombatantOrder()
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

        private Combatant GetTarget(Combatant combatant)
        {
            for (int i = 0; i < combatantOrder.Length; i++)
            {
                if (i == 0)
                {
                    if (combatantOrder[i] == combatant) return combatantOrder[1];
                }
                else
                {
                    if (combatantOrder[i] == combatant) return combatantOrder[0];
                }
            }
            return null;
        }

        public void StartBattle()
        {
            OnChooseFight?.Invoke();

            Round = 0;
            Turn = 0;

            DetermineCombatantOrder();
            foreach (Combatant combatant in combatantOrder)
            {
                if (combatant.Inventory != null)
                {
                    List<Item> items = combatant.Inventory.GetHeldItems();
                    foreach (Item item in items)
                    {
                        foreach (CombatActionBaseData actionData in item.ItemData.OnCombatStartStrategies)
                        {
                            actionData.QueueAction(CombatQueue, combatant, GetTarget(combatant));
                        }
                    }
                }
            }
            CombatQueue.ResolveQueue();
        }

        public void StartNewRound()
        {
            Round += 1;
            Turn = 0;

            DetermineCombatantOrder();
            foreach (Combatant combatant in combatantOrder)
            {
                if (combatant.Inventory != null)
                {
                    List<Item> items = combatant.Inventory.GetHeldItems();
                    foreach (Item item in items)
                    {
                        foreach (CombatActionBaseData actionData in item.ItemData.OnRoundStartStrategies)
                        {
                            actionData.QueueAction(CombatQueue, combatant, GetTarget(combatant));
                        }
                    }
                }
            }
            CombatQueue.ResolveQueue();
        }

        public void StartNewTurn()
        {
            Turn += 1;
            activeCombatanat = combatantOrder[Turn - 1];
            if (activeCombatanat.Inventory != null)
            {
                List<Item> items = activeCombatanat.Inventory.GetHeldItems();
                foreach (Item item in items)
                {
                    foreach (CombatActionBaseData actionData in item.ItemData.OnRoundStartStrategies)
                    {
                        actionData.QueueAction(CombatQueue, activeCombatanat, GetTarget(activeCombatanat));
                    }
                }
            }
            CombatQueue.ResolveQueue();
        }

        public void EndRound()
        {
            DetermineCombatantOrder();
            foreach (Combatant combatant in combatantOrder)
            {
                if (combatant.Inventory != null)
                {
                    List<Item> items = combatant.Inventory.GetHeldItems();
                    foreach (Item item in items)
                    {
                        foreach (CombatActionBaseData actionData in item.ItemData.OnRoundEndStrategies)
                        {
                            actionData.QueueAction(CombatQueue, combatant, GetTarget(combatant));
                        }
                    }
                }
            }
            CombatQueue.ResolveQueue();
        }

        public void DoAttack()
        {
            Combatant attacker = activeCombatanat;
            Combatant defender = GetTarget(activeCombatanat);
            OnAttackStart?.Invoke();
            int attackValue;
            attacker.Attack(out attackValue);
            HitReport hitReport = new HitReport(attackValue);
            defender.ReceiveAttack(hitReport);
            LogBattleAction($"{defender.DisplayName} took {hitReport.Damage} dmg");
            OnAttackEnd?.Invoke();
        }

        private void LogBattleAction(string message)
        {
            OnBattleMessage?.Invoke(message);
            CreateBattleReport();
        }

        #endregion

        public bool CheckForResolution(out BattleReport battleReport)
        {
            battleReport = CreateBattleReport();
            if (battleReport.BattleConcluded)
            {
                OnBattleHasBeenDecided?.Invoke(battleReport);
                finishedCallback(battleReport, Hero, Enemy);
                return true;
            }
            return false;
        }

        private BattleReport CreateBattleReport()
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

            return new BattleReport(battleConcluded, resolution, message);
        }
    }
}