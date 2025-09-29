using System;
using System.Collections.Generic;
using Project.GameplayEffects;
using UnityEngine;

namespace Project
{
    public enum Status {
        Running,
        Complete
    }

    public class EffectQueue
    {
        public List<GameplayEffectStrategy> queuedEffects = new();
        private int currentEffectIndex;
        private bool currentEffectHasStarted = false;

        public void AddEffect(GameplayEffectStrategy effect) => queuedEffects.Add(effect);
        public void RemoveEffect(GameplayEffectStrategy effect) { if (queuedEffects.Contains(effect)) queuedEffects.Remove(effect); }
        public void ClearQueue()
        {
            queuedEffects = new();
            currentEffectIndex = 0;
        }
        public GameplayEffectStrategy GetCurrentEffect()
        {
            if (queuedEffects != null) return queuedEffects[currentEffectIndex];
            return null;
        }

        public event Action OnResolveQueueStart;
        public event Action OnResolveQueueEnd;
        public bool ResolvingQueue = false;

        private Status ResolveQueue()
        {
            while (currentEffectIndex < queuedEffects.Count)
            {
                if (!currentEffectHasStarted)
                {
                    queuedEffects[currentEffectIndex].StartEffect();
                    currentEffectHasStarted = true;
                }
                Status status = queuedEffects[currentEffectIndex].ResolveEffect();
                if (status != Status.Complete)
                {
                    return status;
                }
                queuedEffects[currentEffectIndex].ResetEffect();
                currentEffectIndex++;
                currentEffectHasStarted = false;
            }
            return Status.Complete;
        }

        public void Update()
        {
            if (queuedEffects.Count > 0)
            {
                if (!ResolvingQueue)
                {
                    OnResolveQueueStart?.Invoke();
                    ResolvingQueue = true;
                }
                Status status = ResolveQueue();
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