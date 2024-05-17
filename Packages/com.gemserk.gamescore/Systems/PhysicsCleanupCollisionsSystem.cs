using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class PhysicsCleanupCollisionsSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<PhysicsComponent>, Exc<DisabledComponent>> physicsFilter = default;
        readonly EcsFilterInject<Inc<Physics2dComponent>, Exc<DisabledComponent>> physics2dFilter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var e in physicsFilter.Value)
            {
                ref var physicsComponent = ref physicsFilter.Pools.Inc1.Get(e);
                physicsComponent.collisions.Clear();
                physicsComponent.contactsCount = 0;
            }

            foreach (var e in physics2dFilter.Value)
            {
                ref var physics2dComponent = ref physics2dFilter.Pools.Inc1.Get(e);
                physics2dComponent.contacts.Clear();

                if (physics2dComponent.disableContactsCalculations)
                    continue;
                
                if (physics2dComponent.body != null)
                {
                    Physics2D.GetContacts(physics2dComponent.body, physics2dComponent.contacts);
                }
                else if (!physics2dComponent.isTrigger)
                {
                    Physics2D.GetContacts(physics2dComponent.collideWithDynamicCollider, physics2dComponent.contacts);
                }
            }
        }
    }
}