using System.Collections.Generic;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Controllers
{
    public class State
    {
        public float duration;
        public bool isCompleted;
        public float time;
    }
    
    public struct StatesComponent : IEntityComponent
    {
        public IDictionary<string, State> states;

        public bool HasState(string stateName)
        {
            return states.ContainsKey(stateName);
        }

        public State GetState(string stateName)
        {
            return states[stateName];
        }

        public bool IsActive(string stateName)
        {
            var state = states[stateName];
            return !state.isCompleted;
        }
        
        public void EnterState(string state)
        {
            states[state] = new State
            {
                duration = Mathf.Infinity
            };
        }

        public void EnterState(string state, float duration)
        {
            states[state] = new State
            {
                duration = duration
            };
        }

        public void ExitState(string state)
        {
            states.Remove(state);
        }
    }
}