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
        public int subStatesBitmask;

        public NewState[] states;
        public NewState[] subStates;

        public bool debugTransitions;
        
        // optional for debug
        public GenericTypeCategoryAsset typesAsset;

        public bool HasState(int state)
        {
            return (statesBitmask & state) == state;
        }
    }
    
    public class NewStatesComponentDefinition : ComponentDefinitionBase
    {
        [BitMask(32)]
        public int startingStates;
        
        [BitMask(32)]
        public int startingSubStates;
        
        public bool debugTransitions;

        // optional for debug
        public GenericTypeCategoryAsset typesAsset;
        
        public override string GetComponentName()
        {
            return nameof(NewStateComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new NewStateComponent()
            {
                statesBitmask = startingStates,
                subStatesBitmask = startingSubStates,
                states = new NewState[sizeof(int)],
                subStates = new NewState[sizeof(int)],
                debugTransitions = debugTransitions,
                typesAsset = typesAsset
            });
        }
    }
}