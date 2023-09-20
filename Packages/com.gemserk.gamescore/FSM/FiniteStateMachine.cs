using System.Collections.Generic;
using System.Linq;
using Gemserk.Utilities;

namespace Game.FSM
{
    public interface IFiniteStateMachine<T> where T : class
    {
        public T GetState();

        public ITransition<T> AddTransition(T sourceState, T targetState);
        
        // public ITransition<T> GetTransition(T fromState, T toState);
        //
        // public void RemoveTransition(T fromState, T toState);

        public IList<ITransition<T>> GetTransitions(T t);

        public void Transition(ITransition<T> transition);

        public void Transition(T target);

        public bool InState(T t);
    }

    public interface ITransition<T> where T : class
    {
        public T GetSourceState();
        public T GetTargetState();

        public bool Is(T source, T target);
    }

    public class Transition<T> : ITransition<T> where T : class
    {
        public T source;
        public T target;

        public T GetSourceState()
        {
            return source;
        }

        public T GetTargetState()
        {
            return target;
        }

        public bool Is(T source, T target)
        {
            return this.source.Equals(source) && this.target.Equals(target);
        }
    }

    public class FiniteStateMachine<T> : IFiniteStateMachine<T> where T : class
    {
        public T state;

        private IDictionary<T, IList<ITransition<T>>> transitions = 
            new Dictionary<T, IList<ITransition<T>>>();

        public T GetState()
        {
            return state;
        }

        public ITransition<T> AddTransition(T sourceState, T targetState)
        {
            if (!transitions.ContainsKey(sourceState))
            {
                transitions[sourceState] = new List<ITransition<T>>();
            }

            var newTransition = new Transition<T>()
            {
                source = sourceState,
                target = targetState
            };
            
            transitions[sourceState].Add(newTransition);

            return newTransition;
        }
        
        public ITransition<T> GetTransition(T sourceState, T targetState)
        {
            var transitions = GetTransitions(sourceState);
            return transitions.FirstOrDefault(t => t.GetTargetState().Equals(targetState));
        }

        public IList<ITransition<T>> GetTransitions(T state)
        {
            if (!transitions.ContainsKey(state))
            {
                transitions[state] = new List<ITransition<T>>();
            }

            return transitions[state];
        }

        public void Transition(ITransition<T> transition)
        {
            state = transition.GetTargetState();
        }

        public void Transition(T target)
        {
            Transition(GetTransition(state, target));
        }

        public bool InState(T t)
        {
            return state.Equals(t);
        }
    }

    public static class FiniteStateMachineExtensions
    {
        public static ITransition<T> GetRandomTransition<T>(this IFiniteStateMachine<T> fsm) where T : class
        {
            var transitions = fsm.GetTransitions(fsm.GetState());

            if (transitions.Count == 0)
            {
                return null;
            }

            return transitions.Random();
        }
    }
}