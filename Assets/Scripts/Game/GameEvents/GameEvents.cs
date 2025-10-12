using System;
using System.Collections.Generic;
using Project.Choices;
using UnityEngine;

namespace Project.Core.GameEvents
{

    public interface IGameEvent
    {
        public void SetupEvent();
        public void TeardownEvent();
    }

    public abstract class ChoiceEvent<T> : IGameEvent
    {
        protected int amount;
        public bool IsExitable;
        public event Action OnEventEnded;
        public Choice<T> Choice { get; protected set; }

        public ChoiceEvent(int amount, bool canBeClosedEarly)
        {
            this.amount = amount;
            this.IsExitable = canBeClosedEarly;
        }

        public void SetupEvent()
        {
            GenerateChoices();
        }

        public void TeardownEvent() { }

        public abstract void GenerateChoices();
        public abstract void ChooseItem(int index);
        protected abstract void ResolveCallback(List<T> chosen, List<T> notChosen);
        public abstract void Resolve();
    }
}