using System;
using UnityEngine;
using UnityEngine.Events;

namespace Project.States
{
    public class FuncPredicate : IPredicate
    {
        readonly Func<bool> func;

        public FuncPredicate(Func<bool> func)
        {
            this.func = func;
        }

        public bool Evaluate() => func.Invoke();
    }

    public class ActionPredicate : IPredicate
    {
        bool actionInvoked;
        UnityAction action;

        public ActionPredicate(UnityAction action)
        {
            this.action = action;
            actionInvoked = false;
            this.action += ActionInvoked;
        }

        void ActionInvoked()
        {
            actionInvoked = true;
            Debug.Log("action invoked ( from inside the predicate)");
        }

        public bool Evaluate() => actionInvoked;
    }
}