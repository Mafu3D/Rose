using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.States
{
    public class StateMachine
    {
        StateNode current;
        public StateNode Current { get { return current; } }
        Dictionary<Type, StateNode> nodes = new();
        HashSet<ITransition> anyTransitions = new();

        public void Update()
        {
            ITransition transition = GetTransition();
            if (transition != null)
            {
                ChangeState(transition.To);
            }

            current.State?.Update();
        }

        public void SetState(IState state) {
            current = nodes[state.GetType()];
            current.State.OnEnter();
        }

        public void ChangeState(IState state) {
            if (state == current.State) return;

            var previousState = current.State;
            var nextState = nodes[state.GetType()].State;

            previousState?.OnExit();
            nextState?.OnEnter();
            current = nodes[state.GetType()];
        }

        ITransition GetTransition() {
            foreach (ITransition transition in anyTransitions)
            {
                if (transition.Condition.Evaluate())
                {
                    return transition;
                }
            }

            foreach (ITransition transition in current.Transitions)
            {
                if (transition.Condition.Evaluate())
                {
                    return transition;
                }
            }

            return null;
        }

        public void AddTransition(IState from, IState to, IPredicate condition) {
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
        }

        public void AddAnyTransition(IState to, IPredicate condition) {
            anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));
        }

        StateNode GetOrAddNode(IState state)
        {
            StateNode node = nodes.GetValueOrDefault(state.GetType());

            if (node == null)
            {
                node = new StateNode(state);
                nodes.Add(state.GetType(), node);
            }

            return node;
        }

    }

    public class StateNode
    {
        public IState State { get; }
        public HashSet<ITransition> Transitions { get; }
        public override string ToString() => State.Name;

        public StateNode(IState state)
        {
            State = state;
            Transitions = new HashSet<ITransition>();
        }

        public void AddTransition(IState to, IPredicate condition)
        {
            Transitions.Add(new Transition(to, condition));
        }
    }
}