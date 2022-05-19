using Leopotam.EcsLite;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public class EntityPrefabInstanceSystem : BaseSystem, IEcsRunSystem, IFixedUpdateSystem
    {
        public void Run(EcsSystems systems)
        {
            // TODO: optimize this, maybe auto register by prefab instances or maybe there is no system and 
            // just a factory method called in monobehaviour awake/start.
            var prefabInstances = FindObjectsOfType<EntityPrefabInstance>();

            foreach (var prefabInstance in prefabInstances)
            {
                var parameters = prefabInstance.GetComponentsInChildren<IEntityInstanceParameter>();
                
                var definitionObject = prefabInstance.entityDefinitionPrefab as GameObject;
                var definition = definitionObject.GetComponent<PrefabEntityDefinition>();
                
                world.CreateEntity(definition, parameters);

                prefabInstance.gameObject.SetActive(false);
            }
        }
    }
}