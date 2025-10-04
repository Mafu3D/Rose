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

    public enum BattleState
    {
        NotStarted,
        PreBattle,
        BattleStart,
        RoundStart,
        TurnStart,
        Attack,
        OnHit,
        TurnEnd,
        RoundEnd,
        BattleEnd,
        PostBattle
    }

    public class Battle
    {
        public readonly Character Hero;
        public readonly Character Enemy;
        public int Round;
        public int Turn;
        Character[] combatantOrder = new Character[2];
        Character activeCombatant;
        public BattleState BattleState = BattleState.NotStarted;

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
        private bool currentStateHasBeenEntered;

        public StateMachine StateMachine { get; private set; }
        public CombatQueue CombatQueue { get; private set; }
        public Sequencer CombatSequence { get; private set; }


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


            StateMachine.SetInitialState(new PreBattleState("Pre Battle", StateMachine, GameManager.Instance));
            OnPreBattleStart?.Invoke();
        }

        #region Initiate

        #endregion

        public void Update()
        {
            StateMachine.Update();


            // CombatQueue.Update();
            // if (battleHasStarted)
            // {
            //     CombatSequence.Update();
            // }

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
                CombatQueue.ExecuteNextInQueue();
            }
            else
            {
                ExitState();
                EnterState();
            }

            OnNextActionEvent?.Invoke();
            if (CheckForResolution())
            {
                // BattleReport battleReport = CreateBattleReport();
                // finishedCallback(battleReport, Hero, Enemy);
                // GameManager.Instance.BattleManager.EndActiveBattle();
                BattleState = BattleState.BattleEnd;
            }
        }

        // TODO: Move Enter and Exit State into a state machine or sequencer
        private void EnterState()
        {
            switch (BattleState)
            {
                case BattleState.NotStarted:
                    break;

                case BattleState.PreBattle:
                    break;

                case BattleState.BattleStart:
                    StartBattle();
                    break;

                case BattleState.RoundStart:
                    StartNewRound();
                    break;

                case BattleState.TurnStart:
                    StartNewTurn();
                    break;

                case BattleState.Attack:
                    Debug.Log("Do attack here");
                    break;

                case BattleState.OnHit:
                    Debug.Log("Do on hit here");
                    break;

                case BattleState.TurnEnd:
                    EndTurn();
                    break;

                case BattleState.RoundEnd:
                    EndRound();
                    break;

                case BattleState.BattleEnd:
                    Debug.Log("Do battle end here");
                    break;

                case BattleState.PostBattle:
                    CloseBattle();
                    break;

                default:
                    break;
            }
        }

        private void ExitState()
        {
            switch (BattleState)
            {
                case BattleState.NotStarted:
                    break;

                case BattleState.PreBattle:
                    break;

                case BattleState.BattleStart:
                    BattleState = BattleState.RoundStart;
                    break;

                case BattleState.RoundStart:
                    BattleState = BattleState.TurnStart;
                    break;

                case BattleState.TurnStart:
                    BattleState = BattleState.Attack;
                    break;

                case BattleState.Attack:
                    BattleState = BattleState.OnHit;
                    break;

                case BattleState.OnHit:
                    BattleState = BattleState.TurnEnd;
                    break;

                case BattleState.TurnEnd:
                    if (Turn == 1)
                    {
                        BattleState = BattleState.TurnStart;
                    }
                    else
                    {
                        BattleState = BattleState.RoundEnd;
                    }
                    break;

                case BattleState.RoundEnd:
                    BattleState = BattleState.RoundStart;
                    break;

                case BattleState.BattleEnd:
                    BattleState = BattleState.PostBattle;
                    break;

                case BattleState.PostBattle:
                    break;

                default:
                    break;
            }
        }

        private void StartBattle()
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
            // CombatQueue.ResolveQueue();

            battleHasStarted = true;
        }

        private void StartNewRound()
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
            // CombatQueue.ResolveQueue();
        }

        private void StartNewTurn()
        {
            Turn += 1;
            Debug.Log($"Turn : {Turn}");
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
            // CombatQueue.ResolveQueue();
        }

        private void EndTurn()
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
            // CombatQueue.ResolveQueue();
        }

        private void EndRound()
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
            // CombatQueue.ResolveQueue();
        }

        private void CloseBattle()
        {
            BattleReport battleReport = CreateBattleReport();
            finishedCallback(battleReport, Hero, Enemy);
            GameManager.Instance.BattleManager.EndActiveBattle();
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
    }
}