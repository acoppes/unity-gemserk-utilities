using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Utilities.Pooling;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MyBox;

namespace Game.Systems
{
    public class GameObjectEntitySystem : BaseSystem, 
        IEntityCreatedHandler, IEntityDestroyedHandler, IEcsInitSystem, IEcsRunSystem
    {
        // mark entities to be destroyed when their gameobject destroyed

        private struct GameObjectDisabledComponent : IEntityComponent
        {
            
        }
        
        readonly EcsFilterInject<Inc<GameObjectComponent, DisabledComponent>, Exc<GameObjectDisabledComponent>> 
            toDisableObjects = default;
        
        readonly EcsFilterInject<Inc<GameObjectComponent, GameObjectDisabledComponent>, Exc<DisabledComponent>> 
            toEnableObjects = default;
        
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

        public void Run(EcsSystems systems)
        {
            foreach (var e in toDisableObjects.Value)
            {
                ref var gameObjectComponent = ref toDisableObjects.Pools.Inc1.Get(e);
                gameObjectComponent.gameObject.SetActive(false);
                world.AddComponent(e, new GameObjectDisabledComponent());
            }
            
            foreach (var e in toEnableObjects.Value)
            {
                ref var gameObjectComponent = ref toEnableObjects.Pools.Inc1.Get(e);
                gameObjectComponent.gameObject.SetActive(true);
                toEnableObjects.Pools.Inc2.Del(e);
            }
        }
    }
}