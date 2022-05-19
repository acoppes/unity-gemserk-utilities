using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

public class TimeToLiveSystem : BaseSystem, IEcsRunSystem, IFixedUpdateSystem
{
    readonly EcsFilterInject<Inc<TimeToLiveComponent>> timeToLiveFilter = default;
    readonly EcsPoolInject<TimeToLiveComponent> timeToLiveComponents = default;
    
    public void Run(EcsSystems systems)
    {
        foreach (var entity in timeToLiveFilter.Value)
        {
            ref var timeToLiveComponent = ref timeToLiveComponents.Value.Get(entity);
            timeToLiveComponent.ttl -= Time.deltaTime;
                
            if (timeToLiveComponent.ttl < 0)
            {
                world.AddComponent(entity, new DelayedDestroyComponent());
            }
        }
    }

}