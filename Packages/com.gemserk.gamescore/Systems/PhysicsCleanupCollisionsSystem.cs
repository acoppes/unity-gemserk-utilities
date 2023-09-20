using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace Game.Systems
{
    public class PhysicsCleanupCollisionsSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var physicsComponents = world.GetComponents<PhysicsComponent>();

            foreach (var entity in world.GetFilter<PhysicsComponent>().End())
            {
                ref var physicsComponent = ref physicsComponents.Get(entity);
                physicsComponent.collisions.Clear();
                physicsComponent.contactsCount = 0;
            }
            
            var physics2dComponents = world.GetComponents<Physics2dComponent>();

            foreach (var entity in world.GetFilter<Physics2dComponent>().End())
            {
                ref var physics2dComponent = ref physics2dComponents.Get(entity);
                physics2dComponent.contacts.Clear();
                
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