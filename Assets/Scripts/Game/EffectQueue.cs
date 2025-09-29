using System;
using System.Collections.Generic;
using Project.GameNode;
using Project.GameplayEffects;
using UnityEngine;

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
        public List<GameplayEffectStrategy> queue = new();
        public Dictionary<GameplayEffectStrategy, EffectReferences> registeredEffects = new();
        private int currentEffectIndex;
        private bool currentEffectHasStarted = false;

        public bool QueueEmpty => queue.Count == 0;

        public void QueueEffect(GameplayEffectStrategy effect, Node user, Node target)
        {
            queue.Add(effect);
            EffectReferences references = new EffectReferences(user, target);
            registeredEffects.Add(effect, references);
        }

        public void RemoveEffect(GameplayEffectStrategy effect)
        {
            if (queue.Contains(effect))
            {
                queue.Remove(effect);
                registeredEffects.Remove(effect);
            }
        }
        public void ClearQueue()
        {
            queue = new();
            registeredEffects = new();
            currentEffectIndex = 0;
        }
        public GameplayEffectStrategy GetCurrentEffect()
        {
            if (queue != null) return queue[currentEffectIndex];
            return null;
        }

        public event Action OnResolveQueueStart;
        public event Action OnResolveQueueEnd;
        public bool ResolvingQueue = false;

        private Status IterateQueue()
        {
            while (currentEffectIndex < queue.Count)
            {
                GameplayEffectStrategy currentEffect = queue[currentEffectIndex];
                EffectReferences references;
                registeredEffects.TryGetValue(currentEffect, out references);
                if (!currentEffectHasStarted)
                {
                    currentEffect.StartEffect(references.User, references.Target);
                    currentEffectHasStarted = true;
                }
                Status status = currentEffect.ResolveEffect(references.User, references.Target);
                if (status != Status.Complete)
                {
                    return status;
                }
                currentEffect.ResetEffect(references.User, references.Target);
                currentEffectIndex++;
                currentEffectHasStarted = false;
            }
            return Status.Complete;
        }

        public void Update()
        {
            // ResolveQueue();
        }

        public void ResolveQueue(Action callback)
        {
            if (queue.Count > 0)
            {
                if (!ResolvingQueue)
                {
                    OnResolveQueueStart?.Invoke();
                    ResolvingQueue = true;
                }
                Status status = IterateQueue();
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