using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Game.Components
{
    public class HasLookingIndicatorComponentDefinition : ComponentDefinitionBase
    {
        public Vector3 offset;
        
        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new HasLookingDirectionIndicatorComponent()
            {
                offset = offset
            });
        }
    }
}