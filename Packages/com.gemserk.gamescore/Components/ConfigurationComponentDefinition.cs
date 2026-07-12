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
        
        public bool pendingReconfigure => version != previousVersion;

        public void SetDirty()
        {
            version++;
        }
    }
    
    public struct ConfigurationReconfiguredEvent : IEventComponent
    {
        
    }
}