using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Controllers;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Gemserk.Leopotam.Ecs.Systems
{
    public class DestroyableSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<DestroyableComponent, DelayedDestroyComponent>, Exc<DisabledComponent>> delayedDestroyableFilter = default;
        readonly EcsFilterInject<Inc<DestroyableComponent, DelayedDestroyComponent, DisabledComponent>> disabledDelayedFilter = default;
        readonly EcsFilterInject<Inc<DestroyableComponent>, Exc<DelayedDestroyComponent>> destroyableFilter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var e in delayedDestroyableFilter.Value)
            {
                var destroyable = delayedDestroyableFilter.Pools.Inc1.Get(e);
                if (destroyable.destroy)
                {
                    world.AddComponent(e, new DisabledComponent());
                }
            }
            
            foreach (var e in disabledDelayedFilter.Value)
            {
                var destroyable = disabledDelayedFilter.Pools.Inc1.Get(e);
                ref var delayedDestroy = ref disabledDelayedFilter.Pools.Inc2.Get(e);
                if (destroyable.destroy)
                {
                    if (delayedDestroy.frames < 0)
                    {
                        world.DestroyEntity(world.GetEntity(e));
                    }
                    delayedDestroy.frames--;
                }
            }
            
            foreach (var e in destroyableFilter.Value)
            {
                var destroyable = destroyableFilter.Pools.Inc1.Get(e);
                if (destroyable.destroy)
                {
                    world.DestroyEntity(world.GetEntity(e));
                }
            }
        }
    }
}