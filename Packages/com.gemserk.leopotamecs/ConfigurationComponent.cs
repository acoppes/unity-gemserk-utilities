namespace Gemserk.Leopotam.Ecs
{
    public interface IConfiguration
    {
        void Configure(World world, Entity entity);
    }
    
    public struct ConfigurationComponent : IEntityComponent
    {
        public IConfiguration configuration;
        public int configuredVersion;
        public bool reconfigure;
    }
}