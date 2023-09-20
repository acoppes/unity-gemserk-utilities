using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public interface IDamagedEvent
    {
        void OnDamaged(World world, Entity entity);
    }
    
    public class OnDamageEventControllerSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<ControllerComponent, HealthComponent>, Exc<DisabledComponent>>
            filter = default;

        public void Run(EcsSystems systems)
        {
            foreach (var e in filter.Value)
            {
                ref var controllers = ref filter.Pools.Inc1.Get(e);
                ref var health = ref filter.Pools.Inc2.Get(e);

                if (health.processedDamages.Count == 0)
                {
                    continue;
                }

                foreach (var controller in controllers.controllers)
                {
                    if (controller is IDamagedEvent damaged)
                    {
                        damaged.OnDamaged(world, world.GetEntity(e));
                    }
                }
            }
        }
    }
}