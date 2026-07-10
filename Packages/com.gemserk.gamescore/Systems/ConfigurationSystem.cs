using System;
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
        private readonly EcsFilterInject<Inc<EffectsComponent, ConfigurationComponent, ConfigurationReconfiguredEvent>, 
            Exc<DisabledComponent>> effectsConfigFilter = default;
        
        private readonly Dictionary<string, JsonConfiguration> cachedJsonConfigurations = new Dictionary<string, JsonConfiguration>();

        private const string HealthConfigurationKey = "_health";
        private const string EffectsConfigurationKey = "_effects";
        
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

                if (!string.IsNullOrEmpty(configurationComponent.configurationKey))
                {
                    configurationComponent.configuration = mainJsonConfiguration
                        .GetConfiguration(configurationComponent.configurationKey);
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
                var configuration = healthFilter.Pools.Inc1.Get(e);
                ref var health = ref healthFilter.Pools.Inc2.Get(e);

                try
                {
                   
                    
                    var componentConfiguration = configuration.configuration.GetConfiguration(HealthConfigurationKey);
                    if (componentConfiguration != null)
                    {
                        if (componentConfiguration.Has("total"))
                        {
                            var factor = health.factor;
                            health.total = componentConfiguration.Get<float>("total");
                            health.factor = factor;
                        }

                        if (componentConfiguration.Has("current"))
                        {
                            health.current = componentConfiguration.Get<float>("current");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to configure health for {configuration.configurationKey}: {ex.Message}");
                }
            }
            
            foreach (var e in effectsConfigFilter.Value)
            {
                ref var effects = ref effectsConfigFilter.Pools.Inc1.Get(e);
                var configuration = effectsConfigFilter.Pools.Inc2.Get(e);

                const string valuesKey = "values";

                try
                {
                    if (configuration.configuration.Has(EffectsConfigurationKey))
                    {
                        var componentConfiguration = configuration.configuration.GetConfiguration(EffectsConfigurationKey);

                        if (componentConfiguration.Has(valuesKey))
                        {
                            var effectConfigurations = componentConfiguration.GetConfigurationArray(valuesKey);

                            for (int i = 0; i < effectConfigurations.Length; i++)
                            {
                                if (i < effects.effects.Count)
                                {
                                    var effect = effects.effects[i];
                                    var effectConfiguration = effectConfigurations[i];

                                    if (effectConfiguration.Has("min"))
                                    {
                                        effect.minValue = effectConfiguration.Get<float>("min");
                                    }

                                    if (effectConfiguration.Has("max"))
                                    {
                                        effect.maxValue = effectConfiguration.Get<float>("max");
                                    }

                                    effects.effects[i] = effect;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to configure effects for {configuration.configurationKey}: {ex.Message}");
                }
            }
        }
    }
}