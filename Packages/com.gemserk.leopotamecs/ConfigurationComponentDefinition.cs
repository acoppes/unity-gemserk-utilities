using Gemserk.Utilities;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public class ConfigurationComponentDefinition : ComponentDefinitionBase
    {
        public Object configuration;
        
        public override string GetComponentName()
        {
            return nameof(ConfigurationComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new ConfigurationComponent()
            {
                configuration = configuration.GetInterface<IConfiguration>()
            });
        }
    }
}