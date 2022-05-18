using Leopotam.EcsLite;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public class EntityPrefabInstanceSystem : BaseSystem, IEcsRunSystem, IFixedUpdateSystem
    {
        public void Run(EcsSystems systems)
        {
            var prefabInstances = FindObjectsOfType<EntityPrefabInstance>();

            foreach (var prefabInstance in prefabInstances)
            {
                var parameters = prefabInstance.GetComponentsInChildren<IEntityInstanceParameter>();
                
                var definitionObject = prefabInstance.entityDefinitionPrefab as GameObject;
                var definition = definitionObject.GetComponent<PrefabEntityDefinition>();
                
                world.CreateEntity(definition, parameters);

                // world.AddComponent(entity, new EntityDefinitionComponent
                // {
                //     definition = definition,
                //     parameters = prefabInstance.GetComponentsInChildren<IEntityInstanceParameter>().ToList()
                // });
                
                prefabInstance.gameObject.SetActive(false);
                // Destroy(prefabInstance.gameObject);
            }
        }
    }
}