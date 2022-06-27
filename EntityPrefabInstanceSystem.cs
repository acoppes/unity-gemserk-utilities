using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gemserk.Leopotam.Ecs
{
    public class EntityPrefabInstanceSystem : BaseSystem, IEcsRunSystem, IEcsInitSystem, IEntityDestroyedHandler
    {
        private List<EntityPrefabInstance> prefabInstances = new List<EntityPrefabInstance>();
        
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

                var parameters = prefabInstance.GetComponentsInChildren<IEntityInstanceParameter>();

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
                
                prefabInstance.instance = world.CreateEntity(definition, parameters);

                if (prefabInstance.instanceType == EntityPrefabInstance.InstanceType.InstantiateAndDisable)
                {
                    prefabInstance.gameObject.SetActive(false);
                } else if (prefabInstance.instanceType == EntityPrefabInstance.InstanceType.InstantiateAndLink)
                {
                    world.AddComponent(prefabInstance.instance, new GameObjectComponent
                    {
                        gameObject = prefabInstance.gameObject
                    });
                }
            }
        }



    }
}