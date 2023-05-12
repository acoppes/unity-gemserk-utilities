using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;

namespace Gemserk.Leopotam.Ecs.Systems
{
    public class DestroyableSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var destroyables = world.GetComponents<DestroyableComponent>();
            
            foreach (var entity in world.GetFilter<DestroyableComponent>().End())
            {
                var destroyable = destroyables.Get(entity);
                if (destroyable.destroy)
                {
                    world.DestroyEntity(world.GetEntity(entity));
                }
            }
        }
    }
}