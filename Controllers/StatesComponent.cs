using System.Collections.Generic;

namespace Gemserk.Leopotam.Ecs.Controllers
{
    public class State
    {
        public bool usesDuration;
        public float duration;
        public bool isCompleted;
    }
    
    public struct StatesComponent : IEntityComponent
    {
        public IDictionary<string, State> states;

        public bool HasState(string stateName)
        {
            return states.ContainsKey(stateName);
        }

        public bool IsActive(string stateName)
        {
            var state = states[stateName];
            return !state.isCompleted;
        }
        
        public void EnterState(string state)
        {
            states[state] = new State();
        }

        public void EnterState(string state, float duration)
        {
            states[state] = new State
            {
                usesDuration = true,
                duration = duration
            };
        }

        public void ExitState(string state)
        {
            states.Remove(state);
        }
    }
}