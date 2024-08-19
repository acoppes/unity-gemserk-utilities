using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Game.Components
{
    // used to mark if entities should be disabled by being offscreen or not
    public struct OffScreenDisableComponent : IEntityComponent
    {
        public enum DisableType
        {
            Always = 0,
            FirstTimeOnly = 1
        }
        
        public enum BoundsType
        {
            Fixed = 0,
            NoBounds = 1
            // CopyFromModel = 1
        }
        
        public BoundsType boundsType;
        public Bounds bounds;
        
        // public disabletype => Everyhing, OnlyModel, Nothing

        public DisableType disableType;
        // public int disableCount;
    }
    
    public class OffScreenDisableComponentDefinition : ComponentDefinitionBase
    {
        public OffScreenDisableComponent.BoundsType boundsType;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new OffScreenDisableComponent()
            {
                boundsType = boundsType
            });
        }
    }
}