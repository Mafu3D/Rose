using System;
using System.Collections.Generic;
using Project.GameplayEffects;

namespace Project
{
    public enum Status {
        Running,
        Complete
    }

    public class EffectQueue
    {
        public List<GameplayEffectStrategy> Queue = new List<GameplayEffectStrategy>();
        private int currentEffectIndex;
        private bool currentEffectHasStarted = false;
        public event Action OnResolveQueueStart;
        public event Action OnResolveQueueEnd;
        public bool QueueNeedsToBeResolved => Queue.Count > 0;
        public bool ResolvingQueue = false;

        public GameplayEffectStrategy GetCurrentEffect()
        {
            if (Queue != null) return Queue[currentEffectIndex];
            return null;
        }

        public void AddEffect(GameplayEffectStrategy effect) => Queue.Add(effect);
        public void RemoveEffect(GameplayEffectStrategy effect) { if (Queue.Contains(effect)) Queue.Remove(effect); }
        public void ClearQueue()
        {
            Queue = new();
            currentEffectIndex = 0;
        }

        private Status IterateThroughQueue()
        {
            while (currentEffectIndex < Queue.Count)
            {
                if (!currentEffectHasStarted)
                {
                    Queue[currentEffectIndex].StartEffect();
                    currentEffectHasStarted = true;
                }
                Status status = Queue[currentEffectIndex].ResolveEffect();
                if (status != Status.Complete)
                {
                    return status;
                }
                Queue[currentEffectIndex].ResetEffect();
                currentEffectIndex++;
                currentEffectHasStarted = false;
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

        public void Update()
        {
            if (ResolvingQueue)
            {
                Status status = IterateThroughQueue();
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