using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

public class DelayedDestroySystem : BaseSystem, IEcsRunSystem
{
    readonly EcsFilterInject<Inc<DelayedDestroyComponent>> delayedDestroyFilter = default;
    // readonly EcsPoolInject<DelayedDestroyComponent> delayedDestroyComponents = default;
    
    public void Run(EcsSystems systems)
    {
        foreach (var entity in delayedDestroyFilter.Value)
        {
            // var delayedDestroyComponent = delayedDestroyComponents.Value.Get(entity);
            world.DestroyEntity(world.GetEntity(entity));
        }
    }
}