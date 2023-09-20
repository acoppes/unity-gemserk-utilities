using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;

namespace Platformer.Systems
{
    public class GameObjectEntitySystem : BaseSystem, IEcsRunSystem
    {
        // mark entities to be destroyed when their gameobject destroyed
        
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