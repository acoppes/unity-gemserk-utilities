using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class AttachPointsSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<PositionComponent, AttachPointsComponent>, Exc<DisabledComponent>> filter = default;

        public void Run(EcsSystems systems)
        {
            foreach (var entity in filter.Value)
            {
                var position = filter.Pools.Inc1.Get(entity);
                ref var attachPoints = ref filter.Pools.Inc2.Get(entity);

                foreach (var key in attachPoints.attachPoints.Keys)
                {
                    var attachPoint = attachPoints.attachPoints[key];
                    attachPoint.entityPosition = position.value;
                }
            }
        }


    }
}