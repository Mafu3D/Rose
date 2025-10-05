using System;
using System.Collections.Generic;
using Project.GameNode;
using Project.GameplayEffects;

namespace Project
{
    public enum Status {
        Running,
        Complete
    }

    public struct EffectReferences
    {
        public Node User;
        public Node Target;

        public EffectReferences(Node user, Node target)
        {
            this.User = user;
            this.Target = target;
        }
    }

    public class EffectQueue
    {
        public List<GameplayEffectStrategy> queue = new List<GameplayEffectStrategy>();
        private int currentEffectIndex;
        private bool currentEffectHasStarted = false;
        public event Action OnResolveQueueStart;
        public event Action OnResolveQueueEnd;
        public bool QueueNeedsToBeResolved => queue.Count > 0;
        public bool ResolvingQueue = false;

        public GameplayEffectStrategy GetCurrentEffect()
        {
            if (queue != null) return queue[currentEffectIndex];
            return null;
        }

        public void AddEffect(GameplayEffectStrategy effect) => queue.Add(effect);
        public void RemoveEffect(GameplayEffectStrategy effect) { if (queue.Contains(effect)) queue.Remove(effect); }
        public void ClearQueue()
        {
            queue = new();
            currentEffectIndex = 0;
        }

        private Status IterateThroughQueue()
        {
            while (currentEffectIndex < queue.Count)
            {
                GameplayEffectStrategy currentEffect = queue[currentEffectIndex];
                EffectReferences references;
                registeredEffects.TryGetValue(currentEffect, out references);
                if (!currentEffectHasStarted)
                {
                    queue[currentEffectIndex].StartEffect();
                    currentEffectHasStarted = true;
                }
                Status status = queue[currentEffectIndex].ResolveEffect();
                if (status != Status.Complete)
                {
                    return status;
                }
                queue[currentEffectIndex].ResetEffect();
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
            callback();
        }
    }
}