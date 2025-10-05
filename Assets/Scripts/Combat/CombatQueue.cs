using System;
using System.Collections.Generic;
using Project.Combat.CombatActions;
using UnityEngine;

namespace Project.Combat
{
    public class CombatQueue
    {
        public List<CombatAction> Queue = new List<CombatAction>();
        private int currentQueueIndex;
        public event Action OnResolveQueueStart;
        public event Action OnResolveQueueEnd;
        public event Action<string> OnActionExecuted;
        public bool QueueNeedsToBeResolved => Queue.Count > 0;

        public CombatAction GetCurrentAction()
        {
            if (Queue != null) return Queue[currentQueueIndex];
            return null;
        }

        public void AddAction(CombatAction action) => Queue.Add(action);
        public void RemoveAction(CombatAction action) { if (Queue.Contains(action)) Queue.Remove(action); }
        public void ClearQueue()
        {
            Queue = new();
            currentQueueIndex = 0;
        }

        public void ExecuteNextInQueue()
        {
            Queue[currentQueueIndex].Execute();
            OnActionExecuted?.Invoke(Queue[currentQueueIndex].Message);
            currentQueueIndex++;
            if (currentQueueIndex >= Queue.Count)
            {
                ClearQueue();
            }
        }
    }
}