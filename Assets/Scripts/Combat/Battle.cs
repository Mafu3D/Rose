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
using Project.Attributes;
using Project.Combat.StatusEffects;

namespace Project.Combat
{
    public struct BattleReport
    {
        public bool BattleConcluded;
        public Resolution Resolution;
        public string Message;
        public string Log;
        public BattleReport(bool battleConcluded, Resolution resolution, string message, string log)
        {
            this.BattleConcluded = battleConcluded;
            this.Resolution = resolution;
            this.Message = message;
            this.Log = log;
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

        public string BattleLog { get; private set; } = "";
        public event Action<string> OnBattleLogUpdated;

        public event Action OnBattleInitialize;
        public event Action OnBattleStart;
        public event Action<BattleReport> OnBattleDecided;
        public event Action OnNextActionEvent;
        public event Action GoToNextState;
        public event Action OnNewTurn;
        public event Action OnAttack;

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
            CombatQueue.OnActionExecuted += LogAction;
        }

        public int GetWhichSideIsActiveTEMP()
        {
            if (activeCombatant == Hero) return 0;
            if (activeCombatant == Enemy) return 1;
            else return -1;
        }

        #region Initiate

        public void InitializeFight()
        {
            StateMachine.SetInitialState(new PreBattleState("Pre Battle", StateMachine, GameManager.Instance));
            OnBattleInitialize?.Invoke();

            Hero.ShapshotAttributes();
            Enemy.ShapshotAttributes();
        }

        #endregion

        public void Update()
        {
            StateMachine.Update();
        }

        #region Run
        public void RunAway()
        {
            int heroSpeed = Hero.Attributes.GetAttributeValue(Attributes.AttributeType.Speed);
            int enemySpeed = Enemy.Attributes.GetAttributeValue(Attributes.AttributeType.Speed);

            ranAway = true;

            if (heroSpeed < enemySpeed)
            {
                avoidedRunDamage = false;
                QueueAttack(Enemy, Hero);
            }
            else
            {
                avoidedRunDamage = true;
            }

            // StateMachine.SwitchState(new PostBattleState("Post Battle", StateMachine, GameManager.Instance));
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
            if (Hero == combatant) return Enemy;
            if (Enemy == combatant) return Hero;
            return null;
        }

        #region Battle Loop

        private bool heroBloodied = false;
        private bool enemyBloodied = false;
        private bool heroExposed = false;
        private bool enemyExposed = false;

        public void NextAction()
        {
            bool proceed = true;



            while (proceed)
            {
                if (CombatQueue.QueueNeedsToBeResolved)
                {
                    if (debugMode) Debug.Log("Next Action: Exeucting item in queue...");
                    CombatQueue.ExecuteNextInQueue();
                    CheckExposed();
                    CheckBloodied();
                    proceed = false;
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
                    StateMachine.SwitchState(new BattleEndState("Battle End", StateMachine, GameManager.Instance));
                    proceed = false;
                }
            }
        }

        private void CheckBloodied()
        {
            if (Hero.Attributes.GetAttributeValue(Attributes.AttributeType.Health) <= (Hero.Attributes.GetMaxAttributeValue(Attributes.AttributeType.Health) / 2) && !heroBloodied)
            {
                heroBloodied = true;
                if (Hero.Inventory != null)
                {
                    List<Item> items = Hero.Inventory.GetAllItems(true);
                    foreach (Item item in items)
                    {
                        if (item.ItemData.UsesPerCombat >= 0 && item.Uses >= item.ItemData.UsesPerCombat) continue;
                        item.IncUses();
                        foreach (CombatActionBaseData actionData in item.ItemData.OnSelfBloodiedStrategies)
                        {
                            Debug.Log($"Queueing {item.ItemData.Name} : {actionData.ToString()}");
                            actionData.QueueAction(CombatQueue, Hero, GetTarget(Hero));
                        }
                    }
                }
                if (Enemy.Inventory != null)
                {
                    List<Item> items = Enemy.Inventory.GetAllItems(true);
                    foreach (Item item in items)
                    {
                        if (item.ItemData.UsesPerCombat >= 0 && item.Uses >= item.ItemData.UsesPerCombat) continue;
                        item.IncUses();
                        foreach (CombatActionBaseData actionData in item.ItemData.OnEnemyBloodiedStrategies)
                        {
                            Debug.Log($"Queueing {item.ItemData.Name} : {actionData.ToString()}");
                            actionData.QueueAction(CombatQueue, Enemy, GetTarget(Enemy));
                        }
                    }
                }
            }

            if (Enemy.Attributes.GetAttributeValue(Attributes.AttributeType.Health) <= (Enemy.Attributes.GetMaxAttributeValue(Attributes.AttributeType.Health) / 2) && !enemyBloodied)
            {
                enemyBloodied = true;
                if (Hero.Inventory != null)
                {
                    List<Item> items = Hero.Inventory.GetAllItems(true);
                    foreach (Item item in items)
                    {
                        if (item.ItemData.UsesPerCombat >= 0 && item.Uses >= item.ItemData.UsesPerCombat) continue;
                        item.IncUses();
                        foreach (CombatActionBaseData actionData in item.ItemData.OnEnemyBloodiedStrategies)
                        {
                            Debug.Log($"Queueing {item.ItemData.Name} : {actionData.ToString()}");
                            actionData.QueueAction(CombatQueue, Hero, GetTarget(Hero));
                        }
                    }
                }
                if (Enemy.Inventory != null)
                {
                    List<Item> items = Enemy.Inventory.GetAllItems(true);
                    foreach (Item item in items)
                    {
                        if (item.ItemData.UsesPerCombat >= 0 && item.Uses >= item.ItemData.UsesPerCombat) continue;
                        item.IncUses();
                        foreach (CombatActionBaseData actionData in item.ItemData.OnSelfBloodiedStrategies)
                        {
                            Debug.Log($"Queueing {item.ItemData.Name} : {actionData.ToString()}");
                            actionData.QueueAction(CombatQueue, Enemy, GetTarget(Enemy));
                        }
                    }
                }
            }
        }

        private void CheckExposed()
        {
            if (Hero.Attributes.GetAttributeValue(Attributes.AttributeType.Armor) <= 0 && !heroExposed)
            {
                heroExposed = true;
                if (Hero.Inventory != null)
                {
                    List<Item> items = Hero.Inventory.GetAllItems(true);
                    foreach (Item item in items)
                    {
                        if (item.ItemData.UsesPerCombat >= 0 && item.Uses >= item.ItemData.UsesPerCombat) continue;
                        item.IncUses();
                        foreach (CombatActionBaseData actionData in item.ItemData.OnSelfExposedStrategies)
                        {
                            Debug.Log($"Queueing {item.ItemData.Name} : {actionData.ToString()}");
                            actionData.QueueAction(CombatQueue, Hero, GetTarget(Hero));
                        }
                    }
                }
                if (Enemy.Inventory != null)
                {
                    List<Item> items = Enemy.Inventory.GetAllItems(true);
                    foreach (Item item in items)
                    {
                        if (item.ItemData.UsesPerCombat >= 0 && item.Uses >= item.ItemData.UsesPerCombat) continue;
                        item.IncUses();
                        foreach (CombatActionBaseData actionData in item.ItemData.OnEnemyExposedStrategies)
                        {
                            Debug.Log($"Queueing {item.ItemData.Name} : {actionData.ToString()}");
                            actionData.QueueAction(CombatQueue, Enemy, GetTarget(Enemy));
                        }
                    }
                }
            }

            if (Enemy.Attributes.GetAttributeValue(Attributes.AttributeType.Armor) <= 0 && !enemyExposed)
            {
                enemyExposed = true;
                if (Hero.Inventory != null)
                {
                    List<Item> items = Hero.Inventory.GetAllItems(true);
                    foreach (Item item in items)
                    {
                        if (item.ItemData.UsesPerCombat >= 0 && item.Uses >= item.ItemData.UsesPerCombat) continue;
                        item.IncUses();
                        foreach (CombatActionBaseData actionData in item.ItemData.OnEnemyExposedStrategies)
                        {
                            Debug.Log($"Queueing {item.ItemData.Name} : {actionData.ToString()}");
                            actionData.QueueAction(CombatQueue, Hero, GetTarget(Hero));
                        }
                    }
                }
                if (Enemy.Inventory != null)
                {
                    List<Item> items = Enemy.Inventory.GetAllItems(true);
                    foreach (Item item in items)
                    {
                        if (item.ItemData.UsesPerCombat >= 0 && item.Uses >= item.ItemData.UsesPerCombat) continue;
                        item.IncUses();
                        foreach (CombatActionBaseData actionData in item.ItemData.OnSelfExposedStrategies)
                        {
                            Debug.Log($"Queueing {item.ItemData.Name} : {actionData.ToString()}");
                            actionData.QueueAction(CombatQueue, Enemy, GetTarget(Enemy));
                        }
                    }
                }
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
                    List<Item> items = combatant.Inventory.GetAllItems(true);
                    foreach (Item item in items)
                    {
                        if (item.ItemData.UsesPerCombat >= 0 && item.Uses >= item.ItemData.UsesPerCombat) continue;
                        item.IncUses();
                        foreach (CombatActionBaseData actionData in item.ItemData.OnCombatStartStrategies)
                        {
                            actionData.QueueAction(CombatQueue, combatant, GetTarget(combatant));
                        }
                    }
                }
            }

            if (debugMode) Debug.Log($"Battle Start - InQueue: {CombatQueue.Queue.Count}");

            // If characters start with no armor then they can't be exposed!
            if (Hero.Attributes.GetAttributeValue(Attributes.AttributeType.Armor) <= 0 && !heroExposed)
            {
                heroExposed = true;
            }

            if (Enemy.Attributes.GetAttributeValue(Attributes.AttributeType.Armor) <= 0 && !enemyExposed)
            {
                enemyExposed = true;
            }

            OnBattleStart?.Invoke();
        }

        public void StartNewRound()
        {
            Round += 1;
            Turn = 0;

            DetermineCombatantOrder();
            foreach (Character combatant in combatantOrder)
            {
                // Items
                if (combatant.Inventory != null)
                {
                    List<Item> items = combatant.Inventory.GetAllItems(true);
                    foreach (Item item in items)
                    {
                        if (item.ItemData.UsesPerCombat >= 0 && item.Uses >= item.ItemData.UsesPerCombat) continue;
                        item.IncUses();
                        foreach (CombatActionBaseData actionData in item.ItemData.OnRoundStartStrategies)
                        {
                            actionData.QueueAction(CombatQueue, combatant, GetTarget(combatant));
                        }
                    }
                }

                // Status Effect
                foreach (StatusEffect statusEffect in combatant.StatusEffectManager.GetAllStatusEffects())
                {
                    CombatAction statusEffectAction = statusEffect.OnRoundStart();
                    if (statusEffectAction != null) {
                        CombatQueue.AddAction(statusEffectAction);
                    }
                }
            }

            if (debugMode) Debug.Log($"Start New Round: {Round}, {combatantOrder[0].DisplayName} goes first - InQueue: {CombatQueue.Queue.Count}");
        }

        public void StartNewTurn()
        {
            Turn += 1;

            OnNewTurn?.Invoke();

            // Items
            if (activeCombatant.Inventory != null)
            {
                List<Item> items = activeCombatant.Inventory.GetAllItems(true);
                foreach (Item item in items)
                {
                    if (item.ItemData.UsesPerCombat >= 0 && item.Uses >= item.ItemData.UsesPerCombat) continue;
                    item.IncUses();
                    foreach (CombatActionBaseData actionData in item.ItemData.OnTurnStartStrategies)
                    {
                        actionData.QueueAction(CombatQueue, activeCombatant, GetTarget(activeCombatant));
                    }
                }
            }

            // Status Effect
            foreach (StatusEffect statusEffect in activeCombatant.StatusEffectManager.GetAllStatusEffects())
            {
                CombatAction statusEffectAction = statusEffect.OnTurnStart();
                if (statusEffectAction != null) {
                    CombatQueue.AddAction(statusEffectAction);
                }
            }
            foreach (StatusEffect statusEffect in GetTarget(activeCombatant).StatusEffectManager.GetAllStatusEffects())
            {
                CombatAction statusEffectAction = statusEffect.OnEnemyTurnStart();
                if (statusEffectAction != null) {
                    CombatQueue.AddAction(statusEffectAction);
                }
            }

            if (debugMode) Debug.Log($"Start New Turn: {Turn}, {activeCombatant.DisplayName} - InQueue: {CombatQueue.Queue.Count}");
        }

        public void EndTurn()
        {
            // Items
            if (activeCombatant.Inventory != null)
            {
                List<Item> items = activeCombatant.Inventory.GetAllItems(true);
                foreach (Item item in items)
                {
                    if (item.ItemData.UsesPerCombat >= 0 && item.Uses >= item.ItemData.UsesPerCombat) continue;
                    item.IncUses();
                    foreach (CombatActionBaseData actionData in item.ItemData.OnTurnEndStrategies)
                    {
                        actionData.QueueAction(CombatQueue, activeCombatant, GetTarget(activeCombatant));
                    }
                }
            }

            // Status Effect
            foreach (StatusEffect statusEffect in activeCombatant.StatusEffectManager.GetAllStatusEffects())
            {
                CombatAction statusEffectAction = statusEffect.OnTurnEnd();
                if (statusEffectAction != null)
                {
                    CombatQueue.AddAction(statusEffectAction);
                }
            }
            foreach (StatusEffect statusEffect in GetTarget(activeCombatant).StatusEffectManager.GetAllStatusEffects())
            {
                CombatAction statusEffectAction = statusEffect.OnEnemyTurnEnd();
                if (statusEffectAction != null) {
                    CombatQueue.AddAction(statusEffectAction);
                }
            }

            if (debugMode) Debug.Log($"End Turn: {Turn} - InQueue: {CombatQueue.Queue.Count}");
        }

        public void EndRound()
        {
            // Items
            foreach (Character combatant in combatantOrder)
            {
                if (combatant.Inventory != null)
                {
                    List<Item> items = combatant.Inventory.GetAllItems(true);
                    foreach (Item item in items)
                    {
                        if (item.ItemData.UsesPerCombat >= 0 && item.Uses >= item.ItemData.UsesPerCombat) continue;
                        item.IncUses();
                        foreach (CombatActionBaseData actionData in item.ItemData.OnRoundEndStrategies)
                        {
                            actionData.QueueAction(CombatQueue, combatant, GetTarget(combatant));
                        }
                    }
                }

                // Status effects
                foreach (StatusEffect statusEffect in combatant.StatusEffectManager.GetAllStatusEffects())
                {
                    CombatAction statusEffectAction = statusEffect.OnRoundEnd();
                    if (statusEffectAction != null) {
                        CombatQueue.AddAction(statusEffectAction);
                    }
                }
            }

            if (debugMode) Debug.Log($"End Round: {Round} - InQueue: {CombatQueue.Queue.Count}");
        }

        public void QueueAttackForTurn()
        {
            Character attacker = activeCombatant;
            Character defender = GetTarget(activeCombatant);

            if (attacker.Stunned > 0)
            {
                attacker.Stunned -= 1;
                return;
            }

            QueueAttack(attacker, defender);
        }

        private void QueueAttack(Character attacker, Character defender)
        {
            int attackValue = attacker.GetAttackValue();
            HitReport hitReport = new HitReport(attackValue);

            CombatAction attackAction = new CombatAction(attacker, defender, (attacker, defender) =>
            {
                defender.ReceiveAttack(hitReport);
                OnAttack?.Invoke();

                // Items
                if (attacker.Inventory != null)
                {
                    List<Item> items = attacker.Inventory.GetAllItems(true);
                    foreach (Item item in items)
                    {
                        if (item.ItemData.UsesPerCombat >= 0 && item.Uses >= item.ItemData.UsesPerCombat) continue;
                        item.IncUses();
                        foreach (CombatActionBaseData actionData in item.ItemData.OnHitStrategies)
                        {
                            Debug.Log($"Queueing {item.ItemData.Name} : {actionData.ToString()}");
                            actionData.QueueAction(CombatQueue, attacker, defender);
                        }
                    }
                }

                // Status Effects
                foreach (StatusEffect statusEffect in attacker.StatusEffectManager.GetAllStatusEffects())
                {
                    CombatAction statusEffectAction = statusEffect.OnHit();
                    if (statusEffectAction != null) {
                        CombatQueue.AddAction(statusEffectAction);
                    }
                }

                foreach (StatusEffect statusEffect in defender.StatusEffectManager.GetAllStatusEffects())
                {
                    CombatAction statusEffectAction = statusEffect.OnReceiveHit();
                    if (statusEffectAction != null) {
                        CombatQueue.AddAction(statusEffectAction);
                    }
                }

                if (debugMode) Debug.Log($"Attack: {attacker.DisplayName} attacked {defender.DisplayName} - InQueue: {CombatQueue.Queue.Count}");
            },
            $"{defender.DisplayName} took {hitReport.Damage} dmg");
            CombatQueue.AddAction(attackAction);
        }

        public void EndBattle()
        {
            // Items
            if (ranAway) return;
            foreach (Character combatant in combatantOrder)
            {
                if (combatant.Inventory != null)
                {
                    List<Item> items = combatant.Inventory.GetAllItems(true);
                    foreach (Item item in items)
                    {
                        if (item.ItemData.UsesPerCombat >= 0 && item.Uses >= item.ItemData.UsesPerCombat) continue;
                        item.IncUses();
                        foreach (CombatActionBaseData actionData in item.ItemData.OnCombatEndStrategies)
                        {
                            actionData.QueueAction(CombatQueue, combatant, GetTarget(combatant));
                        }
                    }
                }
            }
        }

        public void FinishBattle()
        {
            BattleReport battleReport = CreateBattleReport();
            OnBattleDecided?.Invoke(battleReport);

            // Reset attributes
            Hero.RestoreSnapshotAttributes();
            Hero.StatusEffectManager.ClearAllStatusEffects();
            Enemy.RestoreSnapshotAttributes();
            Enemy.StatusEffectManager.ClearAllStatusEffects();

            finishedCallback(battleReport, Hero, Enemy);
        }

        public void CloseBattle()
        {
            CombatQueue.OnActionExecuted -= LogAction;
            GameManager.Instance.BattleManager.EndActiveBattle();
        }

        #endregion

        #region Other

        private void LogAction(string message)
        {
            BattleLog += $"{message} \n";
            OnBattleLogUpdated?.Invoke(BattleLog);
        }

        public bool CheckForResolution()
        {
            if (ranAway) return true;
            if (Hero.Attributes.GetAttributeValue(Attributes.AttributeType.Health) <= 0
                || Enemy.Attributes.GetAttributeValue(Attributes.AttributeType.Health) <= 0)
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
            else if (ranAway)
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
                battleConcluded = false;
                resolution = Resolution.None;
                message = $"The battle rages on!";
            }

            return new BattleReport(battleConcluded, resolution, message, BattleLog);
        }

        #endregion
    }
}