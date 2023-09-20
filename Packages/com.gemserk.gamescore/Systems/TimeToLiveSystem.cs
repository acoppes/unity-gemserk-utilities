using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class TimeToLiveSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<TimeToLiveComponent>, Exc<DisabledComponent>> ttlFilter = default;
        readonly EcsFilterInject<Inc<TimeToLiveComponent, DestroyableComponent>, Exc<DisabledComponent>> filter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in ttlFilter.Value)
            {
                ref var timeToLiveComponent = ref ttlFilter.Pools.Inc1.Get(entity);
                timeToLiveComponent.ttl.Increase(dt);
            }
            
            foreach (var entity in filter.Value)
            {
                var timeToLiveComponent = filter.Pools.Inc1.Get(entity);
                ref var destroyableComponent = ref filter.Pools.Inc2.Get(entity);

                if (timeToLiveComponent.ttl.IsReady)
                {
                    destroyableComponent.destroy = true;
                }
            }
        }
    }
}