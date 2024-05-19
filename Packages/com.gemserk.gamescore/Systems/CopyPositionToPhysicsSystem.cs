using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class CopyPositionToPhysicsSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<PhysicsComponent, PositionComponent>, Exc<DisabledComponent>> physicsFilter = default;
        readonly EcsFilterInject<Inc<Physics2dComponent, PositionComponent>, Exc<DisabledComponent>> physics2dFilter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in physicsFilter.Value)
            {
                // copy from position to body
                ref var physicsComponent = ref physicsFilter.Pools.Inc1.Get(entity);
                var positionComponent = physicsFilter.Pools.Inc2.Get(entity);

                if (physicsComponent.collideWithDynamicCollider != null)
                {
                    physicsComponent.collideWithDynamicCollider.enabled = !physicsComponent.disableCollideWithObstacles;
                }
                
                if (physicsComponent.syncType == PhysicsComponent.SyncType.FromPhysics)
                {
                    continue;
                }

                if (physicsComponent.isStatic)
                {
                    physicsComponent.transform.position = positionComponent.value;
                }
                else
                {
                    physicsComponent.body.position = positionComponent.value;
                    physicsComponent.body.velocity = Vector3.zero;
                }
            }
            
            foreach (var entity in physics2dFilter.Value)
            {
                // copy from position to body
                ref var physics2dComponent = ref physics2dFilter.Pools.Inc1.Get(entity);
                var positionComponent = physics2dFilter.Pools.Inc2.Get(entity);

                // if (physicsComponent.collideWithDynamicCollider != null)
                // {
                //     physicsComponent.collideWithDynamicCollider.enabled = !physicsComponent.disableCollideWithObstacles;
                // }
                
                if (physics2dComponent.syncType == PhysicsComponent.SyncType.FromPhysics)
                {
                    continue;
                }

                if (physics2dComponent.isStatic || physics2dComponent.body.bodyType == RigidbodyType2D.Static)
                {
                    physics2dComponent.transform.position = positionComponent.value;
                }
                else
                {
                    physics2dComponent.body.position = positionComponent.value;
                    // physics2dComponent.body.velocity = Vector2.zero;
                }
            }
        }
    }
}