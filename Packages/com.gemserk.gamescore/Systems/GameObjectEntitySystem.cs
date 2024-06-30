using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;

namespace Game.Systems
{
    public class GameObjectEntitySystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler, IEntityDestroyedHandler
    {
        // mark entities to be destroyed when their gameobject destroyed
        
        public void OnEntityCreated(World world, Entity entity)
        {
            if (world.HasComponent<GameObjectComponent>(entity))
            {
                ref var gameObjectComponent = ref world.GetComponent<GameObjectComponent>(entity);
                if (gameObjectComponent.gameObject == null && gameObjectComponent.prefab != null)
                {
                    // TODO: use POOL?
                    gameObjectComponent.gameObject = Instantiate(gameObjectComponent.prefab);
                    gameObjectComponent.gameObject.SetActive(true);
                    gameObjectComponent.createdFromPrefab = true;
                }
            }
        }
        
        public void OnEntityDestroyed(World world, Entity entity)
        {
            if (world.HasComponent<GameObjectComponent>(entity))
            {
                ref var gameObjectComponent = ref world.GetComponent<GameObjectComponent>(entity);
                if (gameObjectComponent.createdFromPrefab && gameObjectComponent.gameObject)
                {
                    Destroy(gameObjectComponent.gameObject);
                    
                    gameObjectComponent.gameObject = null;
                    gameObjectComponent.createdFromPrefab = false;
                }
            }
        }
        
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