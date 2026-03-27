using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public interface IDamagedEvent : IControllerEvent
    {
        void OnDamaged(World world, Entity entity);
    }
    
    public interface IHealedEvent : IControllerEvent
    {
        void OnHealed(World world, Entity entity);
    }
    
    public class OnHealthChangedControllerSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<ControllerComponent, HealthComponent>, Exc<DisabledComponent>>
            filter = default;

        public void Run(EcsSystems systems)
        {
            foreach (var e in filter.Value)
            {
                ref var controllers = ref filter.Pools.Inc1.Get(e);
                ref var health = ref filter.Pools.Inc2.Get(e);

                var hasDamages = health.processedDamages.Count > 0;
                var hasHealEffects = health.processedHealEffects.Count > 0;
                
                if (hasDamages && hasHealEffects)
                {
                    continue;
                }

                foreach (var controller in controllers.controllers)
                {
                    if (hasDamages)
                    {
                        if (controller is IDamagedEvent damaged)
                        {
                            damaged.OnDamaged(world, world.GetEntity(e));
                        }
                    }
                    
                    if (hasHealEffects)
                    {
                        if (controller is IHealedEvent healedEvent)
                        {
                            healedEvent.OnHealed(world, world.GetEntity(e));
                        }
                    }
                }
            }
        }
    }
}