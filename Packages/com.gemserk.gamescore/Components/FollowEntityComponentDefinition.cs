using System;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Game.Components
{
    public struct FollowEntityComponent : IEntityComponent
    {
        [Flags]
        public enum FollowType
        {
            None = 0,
            X = 1 << 0,
            Y = 1 << 1,
            Z = 1 << 2,
        }
        
        public Entity target;
        public Vector3 offset;
        public FollowType followType;
    }
    
    public class FollowEntityComponentDefinition : ComponentDefinitionBase
    {
        public FollowEntityComponent.FollowType followType =
            FollowEntityComponent.FollowType.X | FollowEntityComponent.FollowType.Y | FollowEntityComponent.FollowType.Z;

        // public Vector3 offset;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new FollowEntityComponent
            {
                // offset = offset,
                followType = followType
            });
        }
    }
}