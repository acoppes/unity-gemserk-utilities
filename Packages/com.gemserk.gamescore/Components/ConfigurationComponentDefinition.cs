using Game.Configurations;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using Object = UnityEngine.Object;

namespace Game.Components
{
    public struct ConfigurationComponent : IEntityComponent
    {
        public IConfiguration configuration;
        public int version;
        public int previousVersion;

        public bool pendingReconfigure => version != previousVersion;

        public void SetDirty()
        {
            version++;
        }
    }
    
    public struct ConfigurationReconfiguredEvent : IEventComponent
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