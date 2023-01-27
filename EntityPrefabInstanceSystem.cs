using System.Collections.Generic;
using Gemserk.Utilities;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gemserk.Leopotam.Ecs
{
    public struct EntityPrefabComponent
    {
        public EntityPrefabInstance prefabInstance;
    }
    
    public class EntityPrefabInstanceSystem : BaseSystem, IEcsRunSystem, IEntityDestroyedHandler
    {
        private class GameObjectLinkParameter : IEntityInstanceParameter
        {
            public GameObject gameObject;
            
            public void Apply(World world, Entity entity)
            {
                world.AddComponent(entity, new GameObjectComponent
                {
                    gameObject = gameObject
                });
            }
        }
        
        private readonly List<IEntityInstanceParameter> parameters = new List<IEntityInstanceParameter>();

        public void OnEntityDestroyed(World world, Entity entity)
        {
            if (world.HasComponent<GameObjectComponent>(entity))
            {
                ref var gameObjectComponent = ref world.GetComponent<GameObjectComponent>(entity);
                if (gameObjectComponent.gameObject != null)
                {
                    GameObject.Destroy(gameObjectComponent.gameObject);
                }
                gameObjectComponent.gameObject = null;
            }
        }
        
        public void Run(EcsSystems systems)
        {
            var entityPrefabComponents = world.GetComponents<EntityPrefabComponent>();
            
            foreach (var entity in world.GetFilter<EntityPrefabComponent>().End())
            {
                var entityPrefabComponent = entityPrefabComponents.Get(entity);
                var prefabInstance = entityPrefabComponent.prefabInstance;

                if (prefabInstance.instanceType == EntityPrefabInstance.InstanceType.InstantiateAndLink)
                {
                    if (prefabInstance.instance != Entity.NullEntity)
                    {
                        continue;
                    }
                }

                parameters.Clear();
                prefabInstance.GetComponentsInChildren(parameters);

                var definition = prefabInstance.entityDefinition.GetInterface<IEntityDefinition>();
                
                if (prefabInstance.instanceType == EntityPrefabInstance.InstanceType.InstantiateAndLink)
                {
                    parameters.Add(new GameObjectLinkParameter
                    {
                        gameObject = prefabInstance.gameObject
                    });
                }
                
                prefabInstance.instance = world.CreateEntity(definition, parameters);

                if (prefabInstance.instanceType == EntityPrefabInstance.InstanceType.InstantiateAndDisable)
                {
                    prefabInstance.gameObject.SetActive(false);
                }
                
                if (prefabInstance.instanceType == EntityPrefabInstance.InstanceType.InstantiateAndDestroy)
                {
                    Object.Destroy(prefabInstance.gameObject);
                }
                
                world.DestroyEntity(world.GetEntity(entity));
            }
        }
    }
}