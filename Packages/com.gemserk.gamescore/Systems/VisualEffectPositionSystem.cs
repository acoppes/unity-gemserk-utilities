using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class VisualEffectPositionSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<VfxComponent, PositionComponent>, Exc<DisabledComponent>> filter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in filter.Value)
            {
                ref var vfxComponent = ref filter.Pools.Inc1.Get(entity);
                ref var positionComponent = ref filter.Pools.Inc2.Get(entity);

                if (!world.Exists(vfxComponent.target))
                {
                    continue;
                }

                if (filter.Pools.Inc2.Has(vfxComponent.target))
                {
                    // could use attach points here too?
                    ref var targetPosition = ref filter.Pools.Inc2.Get(vfxComponent.target);
                    positionComponent.value = targetPosition.value;
                    positionComponent.value.z -= 0.01f;
                }
            }
        }
    }
}