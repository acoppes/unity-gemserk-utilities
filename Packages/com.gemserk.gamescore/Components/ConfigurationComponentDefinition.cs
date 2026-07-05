using Game.Configurations;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using Object = UnityEngine.Object;

namespace Game.Components
{
    public struct ConfigurationComponent : IEntityComponent
    {
        public IConfiguration configuration;
        public int configuredVersion;
    }
    
    public struct ConfigurationReconfigureComponent : IActionComponent
    {
        
    }
    
    public class ConfigurationComponentDefinition : ComponentDefinitionBase
    { 
        public Object configurationObject;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new ConfigurationComponent()
            {
                configuration = configurationObject.GetInterface<IConfiguration>()
            });
        }
    }
}