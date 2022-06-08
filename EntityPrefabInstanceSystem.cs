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
            // prefabInstances = FindObjectsOfType<EntityPrefabInstance>();

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
            // TODO: optimize this, maybe auto register by prefab instances or maybe there is no system and 
            // just a factory method called in monobehaviour awake/start.
            // var prefabInstances = FindObjectsOfType<EntityPrefabInstance>();

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