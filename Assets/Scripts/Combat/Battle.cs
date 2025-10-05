using System;
using System.Collections.Generic;
using Project.GameTiles;
using Project.Items;
using UnityEngine;
using Project.GameLoop;
using Project.States;
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
        Character activeCombatant => combatantOrder[Turn - 1];

        Action<BattleReport, Character, Character> finishedCallback;

        public event Action<string> OnBattleMessage;

        public event Action OnPreBattleStart;
        public event Action OnBattleStart;
        public event Action OnBattleHasBeenDecided;
        public event Action OnPostBattleStart;
        public event Action OnNextActionEvent;
        public event Action GoToNextState;

        private bool ranAway;
        private bool avoidedRunDamage;

        public StateMachine StateMachine { get; private set; }
        public CombatQueue CombatQueue { get; private set; }
        public Sequencer CombatSequence { get; private set; }

        private bool debugMode;


        public Battle(Character hero, Character enemy, Action<BattleReport, Character, Character> finished, bool debugMode = false)
        {
            this.Hero = hero;
            this.Enemy = enemy;
            finishedCallback = finished;
            this.debugMode = debugMode;

            StateMachine = new StateMachine();
            CombatQueue = new CombatQueue();


        }

        #region Initiate

        public void StartPreBattle()
        {
            StateMachine.SetInitialState(new PreBattleState("Pre Battle", StateMachine, GameManager.Instance));
            OnPreBattleStart?.Invoke();
        }

        #endregion

        public void Update()
        {
            StateMachine.Update();
        }

        #region Run
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
            if (CombatQueue.QueueNeedsToBeResolved)
            {
                if (debugMode) Debug.Log("Next Action: Exeucting item in queue...");
                CombatQueue.ExecuteNextInQueue();
            }
            else
            {
                if (debugMode) Debug.Log("Next Action: Going to next state...");
                GoToNextState?.Invoke();
            }

            OnNextActionEvent?.Invoke();
            if (CheckForResolution())
            {
                if (debugMode) Debug.Log("Battle has been decided");
                OnBattleHasBeenDecided?.Invoke();
                StateMachine.SwitchState(new PostBattleState("Post Battle", StateMachine, GameManager.Instance));
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

            if (debugMode) Debug.Log($"Battle Start - InQueue: {CombatQueue.Queue.Count}");

            OnBattleStart?.Invoke();
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

            if (debugMode) Debug.Log($"Start New Round: {Round}, {combatantOrder[0].DisplayName} goes first - InQueue: {CombatQueue.Queue.Count}");
        }

        public void StartNewTurn()
        {
            Turn += 1;
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

            if (debugMode) Debug.Log($"Start New Turn: {Turn}, {activeCombatant.DisplayName} - InQueue: {CombatQueue.Queue.Count}");
        }

        public void EndTurn()
        {
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

            if (debugMode) Debug.Log($"End Turn: {Turn} - InQueue: {CombatQueue.Queue.Count}");
        }

        public void EndRound()
        {
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

            if (debugMode) Debug.Log($"End Round: {Round} - InQueue: {CombatQueue.Queue.Count}");
        }

        public void DoAttack()
        {
            Character attacker = activeCombatant;
            Character defender = GetTarget(activeCombatant);
            int attackValue = attacker.GetAttackValue();

            HitReport hitReport = new HitReport(attackValue);
            defender.ReceiveAttack(hitReport);
            LogBattleAction($"{defender.DisplayName} took {hitReport.Damage} dmg");

            if (activeCombatant.Inventory != null)
            {
                List<Item> items = activeCombatant.Inventory.GetHeldItems();
                foreach (Item item in items)
                {
                    foreach (CombatActionBaseData actionData in item.ItemData.OnHitStrategies)
                    {
                        actionData.QueueAction(CombatQueue, activeCombatant, GetTarget(activeCombatant));
                    }
                }
            }

            if (debugMode) Debug.Log($"Attack: {attacker.DisplayName} attacked {defender.DisplayName} - InQueue: {CombatQueue.Queue.Count}");
        }

        public void EndBattle()
        {
            BattleReport battleReport = CreateBattleReport();
            OnPostBattleStart?.Invoke();
            finishedCallback(battleReport, Hero, Enemy);
        }

        public void CloseBattle()
        {
            BattleReport battleReport = CreateBattleReport();
            finishedCallback(battleReport, Hero, Enemy);
            GameManager.Instance.BattleManager.EndActiveBattle();
        }

        #endregion

        #region Other

        private void LogBattleAction(string message)
        {
            OnBattleMessage?.Invoke(message);
            CreateBattleReport();
        }


        private bool CheckForResolution()
        {
            if (Hero.Attributes.GetAttributeValue(Attributes.AttributeType.Health) <= 0)
            {
                return true;
            }
            else if (Enemy.Attributes.GetAttributeValue(Attributes.AttributeType.Health) <= 0)
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

        #endregion
    }
}