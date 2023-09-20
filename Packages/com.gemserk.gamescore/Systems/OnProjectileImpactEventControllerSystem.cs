using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public interface IProjectileImpactEvent
    {
        void OnProjectileImpact(World world, Entity entity);
    }
    
    
    public class OnProjectileImpactEventControllerSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<ControllerComponent, ProjectileComponent>, Exc<DisabledComponent>>
            filter = default;

        public void Run(EcsSystems systems)
        {
            foreach (var e in filter.Value)
            {
                ref var controllers = ref filter.Pools.Inc1.Get(e);
                ref var projectile = ref filter.Pools.Inc2.Get(e);

                if (!projectile.wasImpacted && projectile.impacted)
                {
                    foreach (var controller in controllers.controllers)
                    {
                        if (controller is IProjectileImpactEvent onEvent)
                        {
                            onEvent.OnProjectileImpact(world, world.GetEntity(e));
                        }
                    }
                }
            }
        }
    }
}