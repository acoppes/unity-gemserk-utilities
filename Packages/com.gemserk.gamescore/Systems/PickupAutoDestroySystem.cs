using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class PickupAutoDestroySystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<PickupComponent, DestroyableComponent>> filter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in filter.Value)
            {
                var pickup = filter.Pools.Inc1.Get(entity);
                ref var destroyable = ref filter.Pools.Inc2.Get(entity);
                
                if (pickup.autoDestroyOnPickup && pickup.picked)
                {
                    destroyable.destroy = true;
                }
            }
        }
    }
}