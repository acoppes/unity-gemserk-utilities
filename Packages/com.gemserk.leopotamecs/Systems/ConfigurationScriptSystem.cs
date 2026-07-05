using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Gemserk.Leopotam.Ecs
{
    public class ConfigurationScriptSystem : BaseSystem, IEntityCreatedHandler, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<ConfigurationScriptComponent, ConfigurationScriptReconfigureComponent>, Exc<DisabledComponent>> filter = default;
        
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
            foreach (var e in filter.Value)
            {
                ref var configuration = ref filter.Pools.Inc1.Get(e);
                configuration.configurationScript?.Configure(world, this.GetEntity(e));
                configuration.configuredVersion++;
                filter.Pools.Inc2.Del(e);
            }
        }
    }
}