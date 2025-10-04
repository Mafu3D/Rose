using System;
using System.Collections.Generic;
using Project.Combat.CombatActions;
using UnityEngine;

namespace Project.Combat
{
    public class CombatQueue
    {
        public List<CombatAction> queue = new List<CombatAction>();
        private int currentQueueIndex;
        private bool currentEffectHasStarted = false;
        public event Action OnResolveQueueStart;
        public event Action OnResolveQueueEnd;
        public bool QueueNeedsToBeResolved => queue.Count > 0;

        public CombatAction GetCurrentAction()
        {
            if (queue != null) return queue[currentQueueIndex];
            return null;
        }

        public void AddAction(CombatAction action) => queue.Add(action);
        public void RemoveAction(CombatAction action) { if (queue.Contains(action)) queue.Remove(action); }
        public void ClearQueue()
        {
            queue = new();
            currentQueueIndex = 0;
        }

        public void ExecuteNextInQueue()
        {
            queue[currentQueueIndex].Execute();
            currentQueueIndex++;
            if (currentQueueIndex >= queue.Count)
            {
                ClearQueue();
            }
        }

        public bool TryExecuteNextInQueue()
        {
            if (QueueNeedsToBeResolved)
            {
                queue[currentQueueIndex].Execute();
                currentQueueIndex++;
                if (currentQueueIndex > queue.Count)
                {
                    ClearQueue();

                }
                return true;
            }
            return false;
        }
    }
}