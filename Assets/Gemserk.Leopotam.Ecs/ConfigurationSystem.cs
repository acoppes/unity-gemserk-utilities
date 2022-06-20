using Leopotam.EcsLite;

namespace Gemserk.Leopotam.Ecs
{
    public class ConfigurationSystem : BaseSystem, IEntityCreatedHandler, IEcsRunSystem
    {
        public void OnEntityCreated(World world, Entity entity)
        {
            if (world.HasComponent<ConfigurationComponent>(entity))
            {
                ref var configuration = ref world.GetComponent<ConfigurationComponent>(entity);
                configuration.configuration?.Configure(world, entity);
                configuration.configuredVersion++;
            }
        }

        public void Run(EcsSystems systems)
        {
            var configurations = world.GetComponents<ConfigurationComponent>();
            foreach (var entity in world.GetFilter<ConfigurationComponent>().End())
            {
                ref var configuration = ref configurations.Get(entity);
                if (configuration.reconfigure)
                {
                    configuration.configuration?.Configure(world, entity);
                    configuration.reconfigure = false;
                    configuration.configuredVersion++;
                }
            }
        }
    }
}