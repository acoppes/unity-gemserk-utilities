﻿using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Game.Components
{
    public struct CanPickupComponent : IEntityComponent
    {
        public float range;
        public float sqrRange => range * range;

        // public List<Entity> pickups;
    }
    
    public class CanPickupComponentDefinition : ComponentDefinitionBase
    {
        public float range;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new CanPickupComponent() {
                range = range,
                // pickups = new List<PickupData>()
            });
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}