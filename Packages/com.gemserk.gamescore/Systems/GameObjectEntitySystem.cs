using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Utilities.Pooling;
using Leopotam.EcsLite;
using MyBox;

namespace Game.Systems
{
    public class GameObjectEntitySystem : BaseSystem, IEntityCreatedHandler, IEntityDestroyedHandler, IEcsInitSystem
    {
        // mark entities to be destroyed when their gameobject destroyed
        
        private GameObjectPoolMap poolMap;

        public void Init(EcsSystems systems)
        {
            poolMap = new GameObjectPoolMap("~GameObjects");
        }
        
        public void OnEntityCreated(World world, Entity entity)
        {
            if (world.HasComponent<GameObjectComponent>(entity))
            {
                ref var gameObjectComponent = ref world.GetComponent<GameObjectComponent>(entity);
                if (!gameObjectComponent.gameObject && gameObjectComponent.prefab)
                {
                    // TODO: use POOL?
                    gameObjectComponent.gameObject = gameObjectComponent.reusablePrefab ? 
                        poolMap.Get(gameObjectComponent.prefab) : 
                        Instantiate(gameObjectComponent.prefab);

                    gameObjectComponent.gameObject.name = gameObjectComponent.prefab.name;
                    
                    var entityReference = gameObjectComponent.gameObject.GetOrAddComponent<EntityReference>();
                    entityReference.entity = entity;
                    
                    if (entity.Has<PositionComponent>())
                    {
                        gameObjectComponent.gameObject.transform.position = entity.Get<PositionComponent>().value;
                    }
                    
                    // gameObjectComponent.gameObject = Instantiate(gameObjectComponent.prefab);
                    gameObjectComponent.gameObject.SetActive(true);
                    
                }
            }
        }
        
        public void OnEntityDestroyed(World world, Entity entity)
        {
            if (world.HasComponent<GameObjectComponent>(entity))
            {
                ref var gameObjectComponent = ref world.GetComponent<GameObjectComponent>(entity);
                if (gameObjectComponent.gameObject)
                {
                    if (gameObjectComponent.prefab)
                    {
                        if (gameObjectComponent.reusablePrefab)
                        {
                            poolMap.Release(gameObjectComponent.prefab, gameObjectComponent.gameObject);
                        }
                        else
                        {
                            Destroy(gameObjectComponent.gameObject);
                        }
                    }
                    else
                    {
                        Destroy(gameObjectComponent.gameObject);
                    }
                }
                
                gameObjectComponent.gameObject = null;
            }
        }
    }
}