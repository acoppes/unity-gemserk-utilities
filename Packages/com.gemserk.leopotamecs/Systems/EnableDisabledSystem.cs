using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Gemserk.Leopotam.Ecs.Systems
{
    public class EnableDisabledSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<DisabledComponent, EnableDisabledComponent>> disabledFilter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in disabledFilter.Value)
            {
                var worldEntity = world.GetEntity(entity);
                worldEntity.Remove<EnableDisabledComponent>();
                worldEntity.Remove<DisabledComponent>();
            }
        }
    }
}