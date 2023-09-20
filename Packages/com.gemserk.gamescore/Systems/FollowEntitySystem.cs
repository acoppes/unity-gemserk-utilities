using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class FollowEntitySystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<FollowEntityComponent, PositionComponent>, Exc<DisabledComponent>> filter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in filter.Value)
            {
                var follow = filter.Pools.Inc1.Get(entity);
                ref var positionComponent = ref filter.Pools.Inc2.Get(entity);

                if (!world.Exists(follow.target))
                {
                    continue;
                }

                if (!world.HasComponent<PositionComponent>(follow.target))
                {
                    continue;
                }

                var targetPosition = world.GetComponent<PositionComponent>(follow.target);

                var newPosition = positionComponent.value;

                if (follow.followType.HasFollowType(FollowEntityComponent.FollowType.X))
                {
                    newPosition.x = targetPosition.value.x + follow.offset.x;
                }
                
                if (follow.followType.HasFollowType(FollowEntityComponent.FollowType.Y))
                {
                    newPosition.y = targetPosition.value.y + follow.offset.y;
                }
                
                if (follow.followType.HasFollowType(FollowEntityComponent.FollowType.Z))
                {
                    newPosition.z = targetPosition.value.z + follow.offset.z;
                }

                positionComponent.value = newPosition;
            }
        }
    }
}