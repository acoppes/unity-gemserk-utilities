using System;
using Gemserk.Utilities;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace Gemserk.Leopotam.Ecs
{
    public interface IConfigurationScript
    {
        void Configure(World world, Entity entity);
    }

    public class ConfigurationScriptFunc : IConfigurationScript
    {
        private readonly Action<World, Entity> configurationAction;

        public ConfigurationScriptFunc(Action<World, Entity> configurationAction)
        {
            this.configurationAction = configurationAction;
        }

        public void Configure(World world, Entity entity)
        {
            configurationAction.Invoke(world, entity);
        }
    }
    
    public struct ConfigurationScriptComponent : IEntityComponent
    {
        public IConfigurationScript configurationScript;
        public int configuredVersion;
        public bool reconfigure;
    }
    
    public class ConfigurationComponentDefinition : ComponentDefinitionBase
    {
        [FormerlySerializedAs("configuration")] 
        public Object configurationScript;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new ConfigurationScriptComponent()
            {
                configurationScript = configurationScript.GetInterface<IConfigurationScript>()
            });
        }
    }
}