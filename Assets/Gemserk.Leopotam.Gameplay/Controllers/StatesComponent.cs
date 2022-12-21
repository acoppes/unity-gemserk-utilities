using System;
using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;

namespace Gemserk.Leopotam.Gameplay.Controllers
{
    public class State
    {
        public string name;
        public float time;
    }
    
    public struct StatesComponent : IEntityComponent
    {
        public delegate void StateChangedHandler(StatesComponent statesComponent);

        public IDictionary<string, State> states;
        
        public HashSet<string> activeStates;
        public HashSet<string> previousStates;
        public HashSet<string> statesEntered;
        public HashSet<string> statesExited;

        public event StateChangedHandler onStatesEnterEvent;
        public event StateChangedHandler onStatesExitEvent;

        public bool HasState(string stateName)
        {
            return states.ContainsKey(stateName);
        }

        public State GetState(string stateName)
        {
            return states[stateName];
        }

        public void EnterState(string state)
        {
            activeStates.Add(state);
            states[state] = new State
            {
                name = state,
                time = 0
            };
        }

        public void ExitState(string state)
        {
            activeStates.Remove(state);
            states.Remove(state);
        }

        public static StatesComponent Create()
        {
            return new StatesComponent()
            {
                states = new Dictionary<string, State>(StringComparer.OrdinalIgnoreCase),
                activeStates = new HashSet<string>(StringComparer.OrdinalIgnoreCase),
                previousStates = new HashSet<string>(StringComparer.OrdinalIgnoreCase),
                statesEntered = new HashSet<string>(StringComparer.OrdinalIgnoreCase),
                statesExited = new HashSet<string>(StringComparer.OrdinalIgnoreCase),
            };
        }

        public void OnStatesEnter()
        {
            if (onStatesEnterEvent != null)
            {
                onStatesEnterEvent(this);
            }
        }
        
        public void OnStatesExit()
        {
            if (onStatesExitEvent != null)
            {
                onStatesExitEvent(this);
            }
        }
    }
}