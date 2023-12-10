using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using MyBox;
using UnityEngine;

namespace Game.Components
{
    public struct CopyLookingDirectionFromPhysics : IEntityComponent
    {
        
    }
    
    public class LookingDirectionComponentDefinition : ComponentDefinitionBase
    {
        public enum StartingLookingDirectionType
        {
            Fixed = 0,
            Random2d = 1
        }

        public StartingLookingDirectionType defaultLookingDirectionType = StartingLookingDirectionType.Fixed;
        
        [ConditionalField(nameof(defaultLookingDirectionType), false, StartingLookingDirectionType.Fixed)]
        public Vector3 defaultLookingDirection = Vector3.right;

        public bool copyFromPhysicsRotation;
        
        public override string GetComponentName()
        {
            return nameof(LookingDirection);
        }

        public override void Apply(World world, Entity entity)
        {
            var direction = Vector3.zero;

            if (defaultLookingDirectionType == StartingLookingDirectionType.Fixed)
            {
                direction = defaultLookingDirection;
            } else if (defaultLookingDirectionType == StartingLookingDirectionType.Random2d)
            {
                direction = RandomExtensions.RandomVector2(1, 1, 0, 360);
            }
            
            world.AddComponent(entity, new LookingDirection
            {
                value = direction
            });

            if (copyFromPhysicsRotation)
            {
                world.AddComponent(entity, new CopyLookingDirectionFromPhysics());
            }
        }
    }
}