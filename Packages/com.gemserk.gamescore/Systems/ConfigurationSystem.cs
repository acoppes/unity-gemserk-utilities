using System;
using Game.Components;
using Game.Components.Abilities;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class ConfigurationSystem : BaseSystem, IEcsRunSystem
    {
        public static bool DebugLogConfiguration;
        
        readonly EcsFilterInject<Inc<ConfigurationComponent, ConfigurationReconfiguredEvent>, Exc<DisabledComponent>> reconfigureFilter = default;
        readonly EcsFilterInject<Inc<ConfigurationComponent>, Exc<ConfigurationReconfiguredEvent, DisabledComponent>> pendingFilterCheck = default;
        
        readonly EcsFilterInject<Inc<ConfigurationComponent, HealthComponent, ConfigurationReconfiguredEvent>, Exc<DisabledComponent>> healthFilter = default;
        readonly EcsFilterInject<Inc<ConfigurationComponent, AbilitiesComponent, ConfigurationReconfiguredEvent>, Exc<DisabledComponent>> abilitiesFilter = default;
        readonly EcsFilterInject<Inc<EffectsComponent, ConfigurationComponent, ConfigurationReconfiguredEvent>, 
            Exc<DisabledComponent>> effectsConfigFilter = default;

        private const string HealthConfigurationKey = "_health";
        private const string AbilitiesConfigurationKey = "_abilities";
        private const string EffectsConfigurationKey = "_effects";

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
                    configuration.previousConfiguration = configuration.configuration;
                    
                    if (DebugLogConfiguration)
                    {
                        Debug.Log($"Set dirty {configuration.configurationKey} for reconfigure on start.");
                    }
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
            
            foreach (var e in abilitiesFilter.Value)
            {
                var configuration = abilitiesFilter.Pools.Inc1.Get(e);
                ref var abilities = ref abilitiesFilter.Pools.Inc2.Get(e);

                try
                {
                    var componentConfiguration = configuration.configuration.GetConfiguration(AbilitiesConfigurationKey);
                    if (componentConfiguration != null)
                    {
                        foreach (var ability in abilities.abilities)
                        {
                            if (componentConfiguration.Has(ability.name))
                            {
                                var abilityConfiguration = componentConfiguration.GetConfiguration(ability.name);

                                if (abilityConfiguration.Has("_targeting"))
                                {
                                    var targetingConfiguration = abilityConfiguration.GetConfiguration("_targeting");
                                    
                                    var filter = ability.targeting;

                                    if (targetingConfiguration.Has("min_range"))
                                    {
                                        filter.range.Min = targetingConfiguration.Get<float>("min_range");
                                    }
                                
                                    if (targetingConfiguration.Has("max_range"))
                                    {
                                        filter.range.Max = targetingConfiguration.Get<float>("max_range");
                                    }

                                    ability.targeting = filter;
                                }
                            }
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

                if (DebugLogConfiguration)
                {
                    Debug.Log($"Configuring effects for: {configuration.configurationKey}");
                }

                try
                {
                    if (configuration.configuration.Has(EffectsConfigurationKey))
                    {
                        var componentConfiguration = configuration.configuration.GetConfiguration(EffectsConfigurationKey);

                        if (componentConfiguration.Has(valuesKey))
                        {
                            if (DebugLogConfiguration)
                            {
                                Debug.Log($"Configuring values for: {configuration.configurationKey}.{EffectsConfigurationKey}");
                            }
                            
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
                                        
                                        if (DebugLogConfiguration)
                                        {
                                            Debug.Log($"Read {effect.minValue} min for: {configuration.configurationKey}.{EffectsConfigurationKey}.{valuesKey}");
                                        }
                                    }

                                    if (effectConfiguration.Has("max"))
                                    {
                                        effect.maxValue = effectConfiguration.Get<float>("max");
                                        
                                        if (DebugLogConfiguration)
                                        {
                                            Debug.Log($"Read {effect.maxValue} max for: {configuration.configurationKey}.{EffectsConfigurationKey}.{valuesKey}");
                                        }
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