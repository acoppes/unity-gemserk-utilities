using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Utilities.Pooling;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class DestroyEntityFromGameObjectSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<GameObjectComponent, DestroyableComponent>> destroyablesWithGameObject = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var e in destroyablesWithGameObject.Value)
            {
                ref var gameObjectComponent = ref destroyablesWithGameObject.Pools.Inc1.Get(e);
                ref var destroyableComponent = ref destroyablesWithGameObject.Pools.Inc2.Get(e);

                if (!gameObjectComponent.gameObject)
                {
                    destroyableComponent.destroy = true;
                }
            }
        }

    }
}