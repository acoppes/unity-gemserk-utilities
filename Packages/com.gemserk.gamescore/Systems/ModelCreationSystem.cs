using Game.Components;
using Game.Models;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities.Pooling;
using Leopotam.EcsLite;
using MyBox;

namespace Game.Systems
{
    public class ModelCreationSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler, IEntityDestroyedHandler,
        IEcsInitSystem
    {
        private GameObjectPoolMap poolMap;

        public void Init(EcsSystems systems)
        {
            poolMap = new GameObjectPoolMap("~Models");
        }

        public void Run(EcsSystems systems)
        {
            var modelComponents = world.GetComponents<ModelComponent>();

            foreach (var entity in world.GetFilter<ModelComponent>()
                         .Exc<DisabledComponent>()
                         .End())
            {
                ref var modelComponent = ref modelComponents.Get(entity);
                
                if (modelComponent.prefab != null && modelComponent.instance == null)
                {
                    var modelInstance = poolMap.Get(modelComponent.prefab);

                    if (!modelInstance.HasComponent<EntityReference>())
                    {
                        modelInstance.AddComponent<EntityReference>();
                    }
                    
                    var entityReference = modelInstance.GetComponent<EntityReference>();
                    entityReference.entity = world.GetEntity(entity);

                    modelComponent.instance = modelInstance.GetComponent<Model>();
                }
                
                if (!modelComponent.instance.gameObject.activeSelf)
                {
                    modelComponent.instance.gameObject.SetActive(true);
                }
            }
            
            foreach (var entity in world.GetFilter<ModelComponent>()
                         .Inc<DisabledComponent>()
                         .End())
            {
                ref var modelComponent = ref modelComponents.Get(entity);
                
                if (modelComponent.instance != null && modelComponent.instance.gameObject.activeSelf)
                {
                    modelComponent.instance.gameObject.SetActive(false);
                }
            }
        }
        
        public void OnEntityCreated(World world, Entity entity)
        {
            var modelComponents = world.GetComponents<ModelComponent>();
            if (modelComponents.Has(entity))
            {
                ref var modelComponent = ref modelComponents.Get(entity);
                
                if (modelComponent.prefab != null && modelComponent.instance == null)
                {
                    var modelInstance = poolMap.Get(modelComponent.prefab);

                    if (!modelInstance.HasComponent<EntityReference>())
                    {
                        modelInstance.AddComponent<EntityReference>();
                    }
                    
                    var entityReference = modelInstance.GetComponent<EntityReference>();
                    entityReference.entity = world.GetEntity(entity);

                    modelComponent.instance = modelInstance.GetComponent<Model>();
                    modelComponent.instance.gameObject.SetActive(true);
                }
            }
        }

        public void OnEntityDestroyed(World world, Entity entity)
        {
            var modelComponents = world.GetComponents<ModelComponent>();
            if (modelComponents.Has(entity))
            {
                ref var model = ref modelComponents.Get(entity);
                if (model.instance != null)
                {
                    model.instance.ResetModel();
                    poolMap.Release(model.instance.gameObject);
                }
                model.instance = null;
            }
        }


    }
}