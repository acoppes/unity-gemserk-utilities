using System.Collections.Generic;
using Gemserk.Gameplay;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gemserk.Leopotam.Ecs
{
    public class EntityPrefabInstanceSystem : BaseSystem, IEcsRunSystem, IEcsInitSystem, IEntityDestroyedHandler
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
        
        private List<EntityPrefabInstance> prefabInstances = new List<EntityPrefabInstance>();

        private List<IEntityInstanceParameter> parameters = new List<IEntityInstanceParameter>();
        
        public void Init(EcsSystems systems)
        {
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                foreach (var rootGameObject in scene.GetRootGameObjects())
                {
                    prefabInstances.AddRange(
                        rootGameObject.GetComponentsInChildren<EntityPrefabInstance>(true));
                }
            }
        }
        
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
            foreach (var prefabInstance in prefabInstances)
            {
                if (!prefabInstance.isActiveAndEnabled)
                {
                    continue;
                }
                
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
                
                // IEntityDefinition definition = prefabInstance
                //     .entityDefinition.GetInterface<PrefabEntityDefinition>();
                //
                // // IEntityDefinition definition = definitionObject.GetComponent<PrefabEntityDefinition>();
                //
                // if (definition == null)
                // {
                //     definition = prefabInstance
                //         .entityDefinition.GetInterface<IEntityDefinition>();
                // }

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
                
                // else if (prefabInstance.instanceType == EntityPrefabInstance.InstanceType.InstantiateAndLink)
                // {
                //     world.AddComponent(prefabInstance.instance, new GameObjectComponent
                //     {
                //         gameObject = prefabInstance.gameObject
                //     });
                // }
            }
        }



    }
}