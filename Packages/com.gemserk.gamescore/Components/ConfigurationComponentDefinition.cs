using Game.Configurations;
using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public struct ConfigurationComponent : IEntityComponent
    {
        public IConfiguration configuration;
        public string configurationKey;
        
        public int version;
        public int previousVersion;

        public IConfiguration previousConfiguration;
        
        public bool pendingReconfigure => version != previousVersion || previousConfiguration != configuration;

        public void SetDirty()
        {
            version++;
        }
    }
    
    public struct ConfigurationReconfiguredEvent : IEventComponent
    {
        
    }
}