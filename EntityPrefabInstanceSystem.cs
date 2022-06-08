using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gemserk.Leopotam.Ecs
{
    public class EntityPrefabInstanceSystem : BaseSystem, IEcsRunSystem, IEcsInitSystem
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
        
        public void Run(EcsSystems systems)
        {
            foreach (var prefabInstance in prefabInstances)
            {
                if (!prefabInstance.isActiveAndEnabled)
                    continue;
                
                var parameters = prefabInstance.GetComponentsInChildren<IEntityInstanceParameter>();
                
                var definitionObject = prefabInstance.entityDefinitionPrefab as GameObject;
                IEntityDefinition definition = definitionObject.GetComponent<PrefabEntityDefinition>();

                if (definition == null)
                {
                    definition = definitionObject.GetComponentInChildren<IEntityDefinition>();
                }
                
                world.CreateEntity(definition, parameters);

                prefabInstance.gameObject.SetActive(false);
            }
        }


    }
}