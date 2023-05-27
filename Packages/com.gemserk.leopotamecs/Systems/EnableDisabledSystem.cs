using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Gemserk.Leopotam.Ecs.Systems
{
    public class EnableDisabledSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<DisabledComponent, EnableDisabledComponent>> disabledFilter = default;
        readonly EcsFilterInject<Inc<EnableDisabledComponent>, Exc<DisabledComponent>> enabledFilter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in disabledFilter.Value)
            {
                disabledFilter.Pools.Inc1.Del(entity);
                disabledFilter.Pools.Inc2.Del(entity);
            }
            
            foreach (var entity in enabledFilter.Value)
            {
                enabledFilter.Pools.Inc1.Del(entity);
            }
        }
    }
}