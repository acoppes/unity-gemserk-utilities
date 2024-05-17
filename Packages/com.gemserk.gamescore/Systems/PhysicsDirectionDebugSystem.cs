using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Vertx.Debugging;

namespace Game.Systems
{
    public class PhysicsDirectionDebugSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<PhysicsComponent>, Exc<DisabledComponent>> filter = default;
        
        public void Run(EcsSystems systems)
        {   
#if UNITY_EDITOR
            
            foreach (var e in filter.Value)
            {
                var physicsComponent = filter.Pools.Inc1.Get(e);
                
                if (physicsComponent.isStatic)
                {
                    continue;
                }

                var position3d = physicsComponent.body.position;
                var direction = physicsComponent.body.velocity.normalized;
                
                var position2d = new Vector3(position3d.x, position3d.y + position3d.z * 0.75f);
                var direction2d = new Vector3(direction.x, direction.y + direction.z * 0.75f);

                D.raw(new Shape.Arrow(position3d, direction), Color.red);
                D.raw(new Shape.Arrow(position2d, direction2d), Color.yellow);
            }
#endif
        }
    }
}