using System;
using System.Collections.Generic;
using Project.GameTiles;
using Project.Items;
using UnityEngine;
using Project.GameLoop;
using Project.Combat.CombatStates;
using Project.Combat.CombatActions;
using Project.Sequences;

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
        RanAway,
        Stole,
        Defeat,
        Victory
    }

    public class Battle
    {
        public readonly Character Hero;
        public readonly Character Enemy;
        public int Round;
        public int Turn;
        Character[] combatantOrder = new Character[2];
        Character activeCombatant;

        Action<BattleReport, Character, Character> finishedCallback;

        public event Action<string> OnBattleMessage;

        public event Action OnPreBattleStart;
        public event Action OnBattleStart;
        public event Action OnPostBattleStart;
        public event Action OnAttackStart;
        public event Action OnAttackEnd;
        public event Action OnNextActionEvent;

        private bool ranAway;
        private bool avoidedRunDamage;
        private bool battleHasStarted;

        public StateMachine StateMachine { get; private set; }
        public CombatQueue CombatQueue { get; private set; }
        public Sequence CombatSequence { get; private set; }


        public Battle(Character hero, Character enemy, Action<BattleReport, Character, Character> finished)
        {
            this.Hero = hero;
            this.Enemy = enemy;
            finishedCallback = finished;

            // State Machine
            StateMachine = new StateMachine();
            CombatQueue = new CombatQueue();
            // CombatSequence = new Sequence();

            // RoundStartNode roundStartNode = new RoundStartNode("Round Start", GameManager.Instance);
            // TurnStartNode firstTurnStartNode = new TurnStartNode("First Turn Start", GameManager.Instance);
            // AttackNode firstTurnAttackNode = new AttackNode("First Turn Attack", GameManager.Instance);
            // TurnEndNode firstTurnEndkNode = new TurnEndNode("First Turn End", GameManager.Instance);
            // TurnStartNode secondTurnStartNode = new TurnStartNode("Second Turn Start", GameManager.Instance);
            // AttackNode secondTurnAttackNode = new AttackNode("Second Turn Attack", GameManager.Instance);
            // TurnEndNode secondTurnEndNode = new TurnEndNode("Second Turn End", GameManager.Instance);
            // RoundEndNode roundEndNode = new RoundEndNode("Round End", GameManager.Instance);

            // CombatSequence.AddNode(roundStartNode);
            // CombatSequence.AddNode(firstTurnStartNode);
            // CombatSequence.AddNode(firstTurnAttackNode);
            // CombatSequence.AddNode(firstTurnEndkNode);
            // CombatSequence.AddNode(secondTurnStartNode);
            // CombatSequence.AddNode(secondTurnAttackNode);
            // CombatSequence.AddNode(secondTurnEndNode);
            // CombatSequence.AddNode(roundEndNode);

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
            // StateMachine.Update();
            // if (battleHasStarted)
            // {
            //     CombatSequence.Update();
            // }

        }

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

        public void DetermineCombatantOrder()
        {
            combatantOrder = new Character[2];

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

        private Character GetTarget(Character combatant)
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

        #region Battle Loop

        public void NextAction()
        {
            // Trigger the next action in the queue
            CombatQueue.TriggerNextAction();
            OnNextActionEvent?.Invoke();

            // Check for a resolution to the battle
            BattleReport battleReport;
            bool battleHasBeenDecided = CheckForResolution(out battleReport);
            if (battleHasBeenDecided)
            {
                finishedCallback(battleReport, Hero, Enemy);
                GameManager.Instance.BattleManager.EndActiveBattle();
            }
        }



        public void StartBattle()
        {
            Round = 0;
            Turn = 0;

            DetermineCombatantOrder();
            foreach (Character combatant in combatantOrder)
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

            battleHasStarted = true;
        }

        public void StartNewRound()
        {
            Round += 1;
            Turn = 0;

            DetermineCombatantOrder();
            foreach (Character combatant in combatantOrder)
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
            activeCombatant = combatantOrder[Turn - 1];
            if (activeCombatant.Inventory != null)
            {
                List<Item> items = activeCombatant.Inventory.GetHeldItems();
                foreach (Item item in items)
                {
                    foreach (CombatActionBaseData actionData in item.ItemData.OnTurnStartStrategies)
                    {
                        actionData.QueueAction(CombatQueue, activeCombatant, GetTarget(activeCombatant));
                    }
                }
            }
            CombatQueue.ResolveQueue();
        }

        public void EndTurn()
        {
            activeCombatant = combatantOrder[Turn - 1];
            if (activeCombatant.Inventory != null)
            {
                List<Item> items = activeCombatant.Inventory.GetHeldItems();
                foreach (Item item in items)
                {
                    foreach (CombatActionBaseData actionData in item.ItemData.OnTurnEndStrategies)
                    {
                        actionData.QueueAction(CombatQueue, activeCombatant, GetTarget(activeCombatant));
                    }
                }
            }
            CombatQueue.ResolveQueue();
        }

        public void EndRound()
        {
            // DetermineCombatantOrder();
            foreach (Character combatant in combatantOrder)
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
            // Character attacker = activeCombatant;
            // Character defender = GetTarget(activeCombatant);
            // OnAttackStart?.Invoke();
            // int attackValue;
            // attacker.GetAttackValue(out attackValue);
            // HitReport hitReport = new HitReport(attackValue);
            // defender.ReceiveAttack(hitReport);
            // LogBattleAction($"{defender.DisplayName} took {hitReport.Damage} dmg");
            // OnAttackEnd?.Invoke();
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