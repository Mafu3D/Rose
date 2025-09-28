using System;
using System.Collections.Generic;
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
        public void ClearQueue() => queuedEffects = new();
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
                    queuedEffects[currentEffectIndex].Start();
                    currentEffectHasStarted = true;
                }
                Status status = queuedEffects[currentEffectIndex].Resolve();
                if (status != Status.Complete)
                {
                    return status;
                }
                queuedEffects[currentEffectIndex].Reset();
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
                    Debug.Log("Queue Start");
                    ResolvingQueue = true;
                }
                Status status = ResolveQueue();
                if (status == Status.Complete)
                {
                    OnResolveQueueEnd?.Invoke();
                    Debug.Log("Queue End");
                    ClearQueue();
                    ResolvingQueue = false;
                }
            }
        }
    }
}