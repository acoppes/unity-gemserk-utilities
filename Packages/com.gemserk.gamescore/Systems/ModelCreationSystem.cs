using Game.Components;
using Game.Models;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities.Pooling;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MyBox;
using UnityEngine.Profiling;

namespace Game.Systems
{
    public class ModelCreationSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler, IEntityDestroyedHandler,
        IEcsInitSystem
    {
        readonly EcsFilterInject<Inc<ModelComponent>, Exc<ModelEnabledComponent, DisabledComponent>> filter = default;
        readonly EcsFilterInject<Inc<ModelComponent, ModelEnabledComponent, DisabledComponent>> disabledFilter = default;
        
        private GameObjectPoolMap poolMap;

        public void Init(EcsSystems systems)
        {
            poolMap = new GameObjectPoolMap("~Models");
        }

        public void Run(EcsSystems systems)
        {
            Profiler.BeginSample("ModelSystem");
            
            foreach (var entity in filter.Value)
            {
                ref var modelComponent = ref filter.Pools.Inc1.Get(entity);
                
                if (modelComponent.prefab != null && modelComponent.modelGameObject == null)
                {
                    modelComponent.modelGameObject = poolMap.Get(modelComponent.prefab);

                    if (!modelComponent.modelGameObject.HasComponent<EntityReference>())
                    {
                        modelComponent.modelGameObject.AddComponent<EntityReference>();
                    }
                    
                    var entityReference = modelComponent.modelGameObject.GetComponent<EntityReference>();
                    entityReference.entity = world.GetEntity(entity);

                    modelComponent.instance = modelComponent.modelGameObject.GetComponent<Model>();
                }
                
                if (!modelComponent.modelGameObject.activeSelf)
                {
                    modelComponent.modelGameObject.SetActive(true);
                }

                world.AddComponent(entity, new ModelEnabledComponent());
            }
            
            foreach (var entity in disabledFilter.Value)
            {
                ref var modelComponent = ref disabledFilter.Pools.Inc1.Get(entity);
                
                if (modelComponent.modelGameObject != null && modelComponent.modelGameObject.activeSelf)
                {
                    modelComponent.modelGameObject.SetActive(false);
                }
                
                world.RemoveComponent<ModelEnabledComponent>(entity);
            }
            
            Profiler.EndSample();
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