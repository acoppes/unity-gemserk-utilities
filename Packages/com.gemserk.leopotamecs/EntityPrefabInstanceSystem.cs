using System.Collections.Generic;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Utilities;
using Leopotam.EcsLite;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public struct EntityPrefabComponent
    {
        public BaseEntityPrefabInstance prefabInstance;
    }
    
    public class EntityPrefabInstanceSystem : BaseSystem, IEcsRunSystem
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

                var entityReference = gameObject.AddComponent<EntityReference>();
                entityReference.entity = entity;
            }
        }
        
        private readonly List<IEntityInstanceParameter> parameters = new List<IEntityInstanceParameter>();

        // UPDATE: This is not done by GameObjectEntitySystem
        
        // public void OnEntityDestroyed(World world, Entity entity)
        // {
        //     if (world.HasComponent<GameObjectComponent>(entity))
        //     {
        //         ref var gameObjectComponent = ref world.GetComponent<GameObjectComponent>(entity);
        //         if (gameObjectComponent.gameObject != null)
        //         {
        //             GameObject.Destroy(gameObjectComponent.gameObject);
        //         }
        //         gameObjectComponent.gameObject = null;
        //     }
        // }
        
        public void Run(EcsSystems systems)
        {
            var entityPrefabComponents = world.GetComponents<EntityPrefabComponent>();
            
            foreach (var entity in world.GetFilter<EntityPrefabComponent>().End())
            {
                var entityPrefabComponent = entityPrefabComponents.Get(entity);
                var prefabInstance = entityPrefabComponent.prefabInstance;

                if (prefabInstance.onInstantiateActionType == BaseEntityPrefabInstance.OnInstantiateActionType.LinkObject)
                {
                    if (prefabInstance.instance != Entity.NullEntity)
                    {
                        continue;
                    }
                }

                parameters.Clear();

                var definition = prefabInstance.GetEntityDefinition();

                if (definition != null)
                {
                    prefabInstance.GetEntityParameters(parameters);
                
                    if (prefabInstance.onInstantiateActionType == BaseEntityPrefabInstance.OnInstantiateActionType.LinkObject)
                    {
                        parameters.Add(new GameObjectLinkParameter
                        {
                            gameObject = prefabInstance.gameObject
                        });
                    }
                
                    prefabInstance.instance = world.CreateEntity(definition, parameters);
                }
                else
                {
                    prefabInstance.instance = Entity.NullEntity;
                }

                if (prefabInstance.onInstantiateActionType == BaseEntityPrefabInstance.OnInstantiateActionType.Disable)
                {
                    prefabInstance.gameObject.SetActive(false);
                }
                
                if (prefabInstance.onInstantiateActionType == BaseEntityPrefabInstance.OnInstantiateActionType.Destroy)
                {
                    Object.Destroy(prefabInstance.gameObject);
                }
                
                // this is the temporary spawn entity
                world.DestroyEntity(world.GetEntity(entity));
            }
        }
    }
}