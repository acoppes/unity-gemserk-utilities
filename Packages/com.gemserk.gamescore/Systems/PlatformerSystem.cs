using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class PlatformerSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<PlatformerComponent, GravityComponent, Physics2dComponent>, Exc<DisabledComponent>> filter = default;

        public void Run(EcsSystems systems)
        {
            foreach (var entity in filter.Value)
            {
                ref var platformer = ref filter.Pools.Inc1.Get(entity);
             
                var gravity = filter.Pools.Inc2.Get(entity);
                var physicsComponent = filter.Pools.Inc3.Get(entity);
                
                
                platformer.walking = gravity.inContactWithGround 
                                     && Mathf.Abs(physicsComponent.body.velocity.x) > 0;

            }
        }
    }
}