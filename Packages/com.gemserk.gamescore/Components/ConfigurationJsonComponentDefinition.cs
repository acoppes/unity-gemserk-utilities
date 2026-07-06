using System.IO;
using Game.Configurations;
using Gemserk.Leopotam.Ecs;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Game.Components
{
    public class ConfigurationJsonComponentDefinition : ComponentDefinitionBase
    { 
        public string configurationJsonPath;

        public override void Apply(World world, Entity entity)
        {
            var jsonPath = Path.Combine(Application.streamingAssetsPath, configurationJsonPath);
            var jObject = JObject.Parse(File.ReadAllText(jsonPath));
            
            world.AddComponent(entity, new ConfigurationComponent()
            {
                configuration = new JsonConfiguration(jObject)
            });
        }
    }
}