using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Gemserk.Leopotam.Ecs.Systems
{
    public class EnableDisabledSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<DisabledComponent, EnableEntityComponent>> enableDisabledEntitiesFilter = default;
        readonly EcsFilterInject<Inc<DisableEntityComponent>, Exc<DisabledComponent>> disableEnabledEntitiesFilter = default;
        
        readonly EcsFilterInject<Inc<EnableEntityComponent>, Exc<DisabledComponent>> alreadyEnabledFilter = default;
        readonly EcsFilterInject<Inc<DisableEntityComponent, DisabledComponent>> alreadyDisabledEntities = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in enableDisabledEntitiesFilter.Value)
            {
                enableDisabledEntitiesFilter.Pools.Inc1.Del(entity);
                enableDisabledEntitiesFilter.Pools.Inc2.Del(entity);
            }
            
            foreach (var entity in disableEnabledEntitiesFilter.Value)
            {
                world.AddComponent(entity, new DisabledComponent());
                disableEnabledEntitiesFilter.Pools.Inc1.Del(entity);
            }
            
            // for already enabled entities, consume and ignore the enable action
            foreach (var entity in alreadyEnabledFilter.Value)
            {
                alreadyEnabledFilter.Pools.Inc1.Del(entity);
            }
            
            // for already disabled entities, consume and ignore the disable action.
            foreach (var entity in alreadyDisabledEntities.Value)
            {
                alreadyDisabledEntities.Pools.Inc1.Del(entity);
            }
        }
    }
}