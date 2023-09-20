using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Game.Components
{
    public class LookingDirectionComponentDefinition : ComponentDefinitionBase
    {
        public Vector3 defaultLookingDirection = Vector3.right;
        
        public override string GetComponentName()
        {
            return nameof(LookingDirection);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new LookingDirection
            {
                value = defaultLookingDirection
            });
        }
    }
}