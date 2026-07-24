using System.Collections.Generic;
using System.IO;
using Game.Components;
using Game.Configurations;
using Gemserk.Leopotam.Ecs;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Game.Systems
{
    public class ConfigurationJsonSystem : BaseSystem, IEntityCreatedHandler
    {
        private readonly Dictionary<string, JsonConfiguration> cachedJsonConfigurations = new Dictionary<string, JsonConfiguration>();

        public string defaultJsonConfigurationFilePath;
        
        public void OnEntityCreated(World world, Entity entity)
        {
            if (world.HasComponent<ConfigurationJsonComponent>(entity))
            {
                var configurationJsonComponent = world.GetComponent<ConfigurationJsonComponent>(entity);

                var configJsonPath = configurationJsonComponent.jsonPath;
                
                if (string.IsNullOrEmpty(configJsonPath))
                {
                    configJsonPath = defaultJsonConfigurationFilePath;
                }
                
                if (!cachedJsonConfigurations.ContainsKey(configJsonPath))
                {
                    // load master json path
                    var jsonPath = Path.Combine(Application.streamingAssetsPath, configJsonPath);
                    cachedJsonConfigurations[configJsonPath] =
                        new JsonConfiguration(JObject.Parse(File.ReadAllText(jsonPath)));
                }

                ref var configurationComponent = ref world.GetComponent<ConfigurationComponent>(entity);
                var mainJsonConfiguration = cachedJsonConfigurations[configJsonPath];
                configurationComponent.configuration = mainJsonConfiguration;

                if (!string.IsNullOrEmpty(configurationComponent.configurationKey))
                {
                    configurationComponent.configuration = mainJsonConfiguration
                        .GetConfiguration(configurationComponent.configurationKey);
                    if (configurationComponent.configuration == null)
                    {
                        Debug.LogError($"Failed to get {configurationComponent.configurationKey} from main config file {configJsonPath}");
                    }
                }
            }
            
            if (world.HasComponent<ConfigurationComponent>(entity))
            {
                ref var configuration = ref world.GetComponent<ConfigurationComponent>(entity);
                configuration.version++;
                // world.AddOrSetComponent(entity, new ConfigurationReconfiguredEvent());
            }
        }
    }
}