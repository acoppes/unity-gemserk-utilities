using Leopotam.EcsLite;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Extensions
{
    public class EntityPrefabInstanceSystem : BaseSystem, IEcsRunSystem, IFixedUpdateSystem
    {
        public void Run(EcsSystems systems)
        {
            var prefabInstances = FindObjectsOfType<EntityPrefabInstance>();

            foreach (var prefabInstance in prefabInstances)
            {
                var entity = world.NewEntity();

                var gameObject = prefabInstance.entityDefinitionPrefab as GameObject;
                
                world.AddComponent(entity, new CreateEntity
                {
                    definition = gameObject.GetComponent<PrefabEntityDefinition>()
                });
                
                Destroy(prefabInstance.gameObject);
            }
        }
    }
}