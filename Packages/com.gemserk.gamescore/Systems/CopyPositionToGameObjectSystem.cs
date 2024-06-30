using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class CopyPositionToGameObjectSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<PositionComponent, GameObjectComponent>, Exc<DisabledComponent>> filter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var e in filter.Value)
            {
                ref var position = ref filter.Pools.Inc1.Get(e);
                ref var gameObjectComponent = ref filter.Pools.Inc2.Get(e);

                if (position.type == 0)
                {
                    gameObjectComponent.gameObject.transform.position =
                        GamePerspective.ConvertFromWorld(position.value);
                } else if (position.type == 1)
                {
                    gameObjectComponent.gameObject.transform.position = position.value;
                }
            }
        }
    }
}