using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Controllers;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Gemserk.Leopotam.Ecs.Systems
{
    public class DestroyableSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<DestroyableComponent>> destroyableFilter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in destroyableFilter.Value)
            {
                var destroyable = destroyableFilter.Pools.Inc1.Get(entity);
                if (destroyable.destroy)
                {
                    world.DestroyEntity(world.GetEntity(entity));
                }
            }
        }
    }
}