using Leopotam.EcsLite;

namespace Gemserk.Leopotam.Ecs
{
    public class ConfigurationScriptSystem : BaseSystem, IEntityCreatedHandler, IEcsRunSystem
    {
        public void OnEntityCreated(World world, Entity entity)
        {
            if (world.HasComponent<ConfigurationScriptComponent>(entity))
            {
                ref var configuration = ref world.GetComponent<ConfigurationScriptComponent>(entity);
                configuration.configurationScript?.Configure(world, entity);
                configuration.configuredVersion++;
            }
        }

        public void Run(EcsSystems systems)
        {
            var configurations = world.GetComponents<ConfigurationScriptComponent>();
            foreach (var entity in world.GetFilter<ConfigurationScriptComponent>()
                         .Exc<DisabledComponent>()
                         .End())
            {
                ref var configuration = ref configurations.Get(entity);
                if (configuration.reconfigure)
                {
                    configuration.configurationScript?.Configure(world, this.GetEntity(entity));
                    configuration.reconfigure = false;
                    configuration.configuredVersion++;
                }
            }
        }
    }
}