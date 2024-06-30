using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Utilities.Pooling;
using Leopotam.EcsLite;

namespace Game.Systems
{
    public class DestroyEntityFromGameObjectSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var gameObjectComponents = world.GetComponents<GameObjectComponent>();
            var destroyableComponents = world.GetComponents<DestroyableComponent>();

            foreach (var entity in world.GetFilter<GameObjectComponent>()
                         .Inc<DestroyableComponent>()
                         .End())
            {
                var gameObjectComponent = gameObjectComponents.Get(entity);
                ref var destroyableComponent = ref destroyableComponents.Get(entity);

                if (gameObjectComponent.gameObject == null)
                {
                    destroyableComponent.destroy = true;
                }
            }
        }

    }
}