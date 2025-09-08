using System;
using System.Collections.Generic;

namespace Game.Service
{
    public class StateMachine
    {
        StateNode current;
        Dictionary<Type, StateNode> nodes = new Dictionary<Type, StateNode>();
        HashSet<ITransition> anyTransitions = new HashSet<ITransition>();
        public void Update()
        {
            var transition = GetTransition();
            if (transition != null)
            {
                ChangeState(transition.To);
            }
            current.State?.Update();
        }
        public void SetState(IState state)
        {
            current = GetOrAddNode(state);
            current.State?.OnEnter();

        }
        public void ChangeState(IState state)
        {
            if (current == state) return;
            var predicateState = current.State;
            var nextState =GetOrAddNode(state).State;

            predicateState?.OnExit();
            nextState?.OnEnter();
            current = GetOrAddNode(state);

        }

        ITransition GetTransition()
        {
            foreach (var transition in anyTransitions)
            {
                if (transition.Condition.Evaluate())
                {
                    return transition;
                }
            }
            foreach(var transition in current.Transitions)
            {
                if (transition.Condition.Evaluate())
                {
                    return transition;
                }
            }
            return null;    
        }
        public void AddAnyTransition(IState to, IPredicate condition)
        {
            anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));  
        }
        
        public void AddTransition(IState from, IState to, IPredicate condition)
        {
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
        }

        StateNode GetOrAddNode(IState state)
        {
            var node = nodes.GetValueOrDefault(state.GetType());
            if (node == null)
            {
                node = new StateNode(state);
                nodes.Add(state.GetType(), node);
            }
            return node;
        }
        class StateNode { 
            public IState State { get; }
            public HashSet<ITransition> Transitions { get; }

           
            public StateNode(IState state)
            {
                this.State = state;
                Transitions = new HashSet<ITransition>();
            }

            public void AddTransition(IState to, IPredicate condition)
            {
                Transitions.Add(new Transition(to, condition));

            }
        }

    }
}