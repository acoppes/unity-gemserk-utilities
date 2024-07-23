using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class CopyPositionToGameObjectSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<PositionComponent, GameObjectComponent, CopyPositionFromEntityComponent>, Exc<DisabledComponent>> fromEntity = default;
        readonly EcsFilterInject<Inc<PositionComponent, GameObjectComponent, CopyPositionFromGameObjectComponent>, Exc<DisabledComponent>> fromGameObject = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var e in fromEntity.Value)
            {
                ref var position = ref fromEntity.Pools.Inc1.Get(e);
                ref var gameObjectComponent = ref fromEntity.Pools.Inc2.Get(e);

                if (position.type == 0)
                {
                    gameObjectComponent.gameObject.transform.position =
                        GamePerspective.ConvertFromWorld(position.value);
                } else if (position.type == 1)
                {
                    gameObjectComponent.gameObject.transform.position = position.value;
                }
            }
            
            foreach (var e in fromGameObject.Value)
            {
                ref var position = ref fromEntity.Pools.Inc1.Get(e);
                ref var gameObjectComponent = ref fromEntity.Pools.Inc2.Get(e);

                position.value = gameObjectComponent.gameObject.transform.position;
            }
        }
    }
}