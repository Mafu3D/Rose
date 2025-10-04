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
        public bool ResolvingQueue = false;
        private bool nextActionTriggered = false;

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

        private Status IterateThroughQueue()
        {
            while (currentQueueIndex < queue.Count)
            {
                // if (!currentEffectHasStarted)
                // {
                //     queue[currentQueueIndex].StartEffect();
                //     currentEffectHasStarted = true;
                // }
                // Status status = queue[currentQueueIndex].ResolveEffect();
                // if (status != Status.Complete)
                // {
                //     return status;
                // }
                // queue[currentQueueIndex].ResetEffect();
                // currentQueueIndex++;
                // currentEffectHasStarted = false;

                queue[currentQueueIndex].Execute();
                currentQueueIndex++;
                return Status.Running;
            }
            return Status.Complete;
        }

        private Status ExecuteNextInQueue()
        {
            queue[currentQueueIndex].Execute();
            currentQueueIndex++;
            if (currentQueueIndex < queue.Count)
            {
                return Status.Running;
            }
            return Status.Complete;
        }

        public void ResolveQueue()
        {
            if (QueueNeedsToBeResolved)
            {
                ResolvingQueue = true;
                OnResolveQueueStart?.Invoke();
            }
        }

        public void TriggerNextAction()
        {
            nextActionTriggered = true;
        }

        public void Update()
        {
            if (ResolvingQueue && nextActionTriggered)
            {
                Debug.Log("resolving queue");
                nextActionTriggered = false;
                Status status = ExecuteNextInQueue();
                if (status == Status.Complete)
                {
                    OnResolveQueueEnd?.Invoke();
                    ClearQueue();
                    ResolvingQueue = false;
                }
            }
        }
    }
}