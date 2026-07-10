using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public struct ConfigurationJsonComponent : IEntityComponent
    {
        public string jsonPath;
    }
    
    public class ConfigurationJsonComponentDefinition : ComponentDefinitionBase
    { 
        public string configurationJsonPath;
        public string configurationKey;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new ConfigurationComponent()
            {
                configurationKey = configurationKey
            });
            world.AddComponent(entity, new ConfigurationJsonComponent()
            {
                jsonPath = configurationJsonPath
            });
        }
    }
}