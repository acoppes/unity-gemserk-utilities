using System;
using System.Collections.Generic;

namespace Gemserk.Leopotam.Ecs.Controllers
{
    public class State
    {
        public string name;
        public float time;
        public float duration;
        public int updateCount;
    }
    
    public struct StatesComponent : IEntityComponent
    {
        public delegate void StateChangedHandler(StatesComponent statesComponent);

        public IDictionary<string, State> states;
        
        public HashSet<string> activeStates;
        public HashSet<string> previousStates;
        public HashSet<string> statesEntered;
        public HashSet<string> statesExited;

        public bool debugTransitions;

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
        
        public bool TryGetState(string stateName, out State state)
        {
            return states.TryGetValue(stateName, out state);
        }

        public void EnterState(string state, float duration = 0)
        {
            activeStates.Add(state);
            states[state] = new State
            {
                name = state,
                time = 0,
                duration = duration
            };
        }

        public void ExitState(string state)
        {
            activeStates.Remove(state);
            states.Remove(state);
        }
        
        public void ExitState(string stateName, float time)
        {
            if (states.TryGetValue(stateName, out var state))
            {
                state.duration = state.time + time;
            }
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

        public void ClearCallbacks()
        {
            onStatesEnterEvent = null;
            onStatesExitEvent = null;
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