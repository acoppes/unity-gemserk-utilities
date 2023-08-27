using Gemserk.BitmaskTypes;
using Gemserk.Leopotam.Ecs;

namespace MyGame
{
    public struct StateV2
    {
        // public string name;
        // public bool active;
        
        public float time;
        public float duration;
        public int updateCount;
    }
    
    public struct StatesComponentV2 : IEntityComponent
    {
        public delegate void StateChangedHandler(StatesComponentV2 statesComponent);
        
        public int statesBitmask;
        
        public int previousStatesBitmask;
        
        public int statesEnteredLastFrame;
        public int statesExitedLastFrame;
        
        // public int subStatesBitmask;

        public StateV2[] states;
       // public NewState[] subStates;

        public bool debugTransitions;
        
        // optional for debug
        public TypeSetAsset typesAsset;

        public event StateChangedHandler onStatesEnterEvent;
        public event StateChangedHandler onStatesExitEvent;
        
        public static StatesComponentV2 Create()
        {
            return new StatesComponentV2()
            {
                statesBitmask = 0,
                states = new StateV2[sizeof(int) * 8]
            };
        }

        public bool HasState(int state)
        {
            var stateMask = 1 << state;
            return (statesBitmask & stateMask) == stateMask;
        }

        public void Enter(int state)
        {
            var stateMask = 1 << state;
            statesBitmask |= stateMask;
            states[state] = new StateV2();
        }
        
        public void Exit(int state)
        {
            var stateMask = 1 << state;
            statesBitmask &= ~stateMask;
        }
        
        public bool HasEnteredInLastFrame(int state)
        {
            var stateMask = 1 << state;
            return (statesEnteredLastFrame & stateMask) == stateMask;
        }
        
        public bool HasExitLastFrame(int state)
        {
            var stateMask = 1 << state;
            return (statesExitedLastFrame & stateMask) == stateMask;
        }
        
        public void ClearCallbacks()
        {
            onStatesEnterEvent = null;
            onStatesExitEvent = null;
        }
    }
    
    public class StatesComponentV2Definition : ComponentDefinitionBase
    {
        // [BitMask(32)]
        // public int startingStates;
        
        public bool debugTransitions;

        public TypeSetAsset typesAsset;
        
        public override string GetComponentName()
        {
            return nameof(StatesComponentV2);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new StatesComponentV2()
            {
                statesBitmask = 0,
                states = new StateV2[sizeof(int) * 8],
                debugTransitions = debugTransitions,
                typesAsset = typesAsset
            });
        }
    }
}