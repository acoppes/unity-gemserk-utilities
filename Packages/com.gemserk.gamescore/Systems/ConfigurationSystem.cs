using Game.Components;
using Game.Configurations;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class ConfigurationSystem : BaseSystem, IEntityCreatedHandler, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<ConfigurationComponent, ConfigurationReconfigureComponent>, Exc<DisabledComponent>> filter = default;
        readonly EcsFilterInject<Inc<ConfigurationComponent, HealthComponent, ConfigurationReconfigureComponent>, Exc<DisabledComponent>> healthFilter = default;
        readonly EcsFilterInject<Inc<ConfigurationReconfigureComponent>, Exc<DisabledComponent>> reconfigureFilter = default;
        
        public void OnEntityCreated(World world, Entity entity)
        {
            if (world.HasComponent<ConfigurationComponent>(entity))
            {
                world.AddOrSetComponent(entity, new ConfigurationReconfigureComponent());
            }
        }

        public void Run(EcsSystems systems)
        {
            foreach (var e in healthFilter.Value)
            {
                var configurationComponent = healthFilter.Pools.Inc1.Get(e);
                ref var health = ref healthFilter.Pools.Inc2.Get(e);

                var configuration = configurationComponent.configuration;
                {
                    var healthConfiguration = configuration.Get<IConfiguration>("health");
                    if (healthConfiguration != null)
                    {
                        if (healthConfiguration.Has("total"))
                        {
                            health.total = healthConfiguration.Get<float>("total");
                        }
                        
                        if (healthConfiguration.Has("current"))
                        {
                            health.current = healthConfiguration.Get<float>("current");
                        }
                    }
                }
            }
            
            foreach (var e in filter.Value)
            {
                ref var configuration = ref filter.Pools.Inc1.Get(e);
                configuration.configuredVersion++;
                filter.Pools.Inc2.Del(e);
            }
        }
    }
}