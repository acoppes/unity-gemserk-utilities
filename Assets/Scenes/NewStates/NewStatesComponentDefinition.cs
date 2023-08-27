using Gemserk.BitmaskTypes;
using Gemserk.Leopotam.Ecs;

namespace MyGame
{
    public struct NewState
    {
        // public string name;
        // public bool active;
        
        public float time;
        public float duration;
        public int updateCount;
    }
    
    public struct NewStateComponent : IEntityComponent
    {
        public int statesBitmask;
        // public int subStatesBitmask;

        public NewState[] states;
       // public NewState[] subStates;

        public bool debugTransitions;
        
        // optional for debug
        public TypeSetAsset typesAsset;

        public bool HasState(int state)
        {
            var stateMask = 1 << state;
            return (statesBitmask & stateMask) == stateMask;
        }

        public void Enter(int state)
        {
            var stateMask = 1 << state;
            statesBitmask |= stateMask;
            states[state] = new NewState();
        }
        
        public void Exit(int state)
        {
            var stateMask = 1 << state;
            statesBitmask &= ~stateMask;
        }
    }
    
    public class NewStatesComponentDefinition : ComponentDefinitionBase
    {
        [BitMask(32)]
        public int startingStates;
        
        //[BitMask(32)]
        //public int startingSubStates;
        
        public bool debugTransitions;

        // optional for debug
        public TypeSetAsset typesAsset;
        
        public override string GetComponentName()
        {
            return nameof(NewStateComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new NewStateComponent()
            {
                statesBitmask = startingStates,
                //subStatesBitmask = startingSubStates,
                states = new NewState[sizeof(int) * 8],
                //subStates = new NewState[sizeof(int)],
                debugTransitions = debugTransitions,
                typesAsset = typesAsset
            });
        }
    }
}