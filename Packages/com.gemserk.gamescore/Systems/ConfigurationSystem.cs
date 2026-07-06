using System.Collections.Generic;
using System.IO;
using Game.Components;
using Game.Configurations;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Game.Systems
{
    public class ConfigurationSystem : BaseSystem, IEntityCreatedHandler, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<ConfigurationComponent, HealthComponent, ConfigurationReconfiguredEvent>, Exc<DisabledComponent>> healthFilter = default;
        readonly EcsFilterInject<Inc<ConfigurationComponent, ConfigurationReconfiguredEvent>, Exc<DisabledComponent>> reconfigureFilter = default;
        readonly EcsFilterInject<Inc<ConfigurationComponent>, Exc<ConfigurationReconfiguredEvent, DisabledComponent>> pendingFilterCheck = default;

        private readonly Dictionary<string, JsonConfiguration> cachedJsonConfigurations = new Dictionary<string, JsonConfiguration>();
        
        public void OnEntityCreated(World world, Entity entity)
        {
            if (world.HasComponent<ConfigurationJsonComponent>(entity))
            {
                var configurationJsonComponent = world.GetComponent<ConfigurationJsonComponent>(entity);
                if (!cachedJsonConfigurations.ContainsKey(configurationJsonComponent.jsonPath))
                {
                    // load master json path
                    var jsonPath = Path.Combine(Application.streamingAssetsPath, configurationJsonComponent.jsonPath);
                    cachedJsonConfigurations[configurationJsonComponent.jsonPath] =
                        new JsonConfiguration(JObject.Parse(File.ReadAllText(jsonPath)));
                }

                ref var configurationComponent = ref world.GetComponent<ConfigurationComponent>(entity);
                var mainJsonConfiguration = cachedJsonConfigurations[configurationJsonComponent.jsonPath];
                configurationComponent.configuration = mainJsonConfiguration;

                if (!string.IsNullOrEmpty(configurationJsonComponent.configurationKey))
                {
                    configurationComponent.configuration = mainJsonConfiguration
                        .GetConfiguration(configurationJsonComponent.configurationKey);
                }
            }
            
            if (world.HasComponent<ConfigurationComponent>(entity))
            {
                ref var configuration = ref world.GetComponent<ConfigurationComponent>(entity);
                configuration.version++;
                // world.AddOrSetComponent(entity, new ConfigurationReconfiguredEvent());
            }
        }

        public void Run(EcsSystems systems)
        {
            // this is for next loop, to clear the events
            foreach (var e in reconfigureFilter.Value)
            {
                // ref var configuration = ref pendingFilterCheck.Pools.Inc1.Get(e);
                reconfigureFilter.Pools.Inc2.Del(e);
            }
            
            foreach (var e in pendingFilterCheck.Value)
            {
                ref var configuration = ref pendingFilterCheck.Pools.Inc1.Get(e);
                if (configuration.pendingReconfigure)
                {
                    world.AddComponent(e, new ConfigurationReconfiguredEvent());
                    configuration.previousVersion = configuration.version;
                }
            }
            
            foreach (var e in healthFilter.Value)
            {
                var configurationComponent = healthFilter.Pools.Inc1.Get(e);
                ref var health = ref healthFilter.Pools.Inc2.Get(e);

                var configuration = configurationComponent.configuration;
                {
                    var healthConfiguration = configuration.GetConfiguration("health");
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
            

        }
    }
}