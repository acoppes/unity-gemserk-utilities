using Game.Components;
using Game.Models;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities.Pooling;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MyBox;

namespace Game.Systems
{
    public class ModelCreationSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler, IEntityDestroyedHandler,
        IEcsInitSystem
    {
        readonly EcsFilterInject<Inc<ModelComponent>, Exc<ModelInstanceComponent, DisabledComponent>> filter = default;
        readonly EcsFilterInject<Inc<ModelComponent, ModelInstanceComponent, ModelEnabledComponent, DisabledComponent>> disabledFilter = default;
        
        private GameObjectPoolMap poolMap;

        public void Init(EcsSystems systems)
        {
            poolMap = new GameObjectPoolMap("~Models");
        }

        public void Run(EcsSystems systems)
        {
            // Profiler.BeginSample("ModelSystem");
            
            foreach (var entity in filter.Value)
            {
                ref var model = ref filter.Pools.Inc1.Get(entity);
                CreateModel(entity, ref model);
            }
            
            foreach (var e in disabledFilter.Value)
            {
                ref var model = ref disabledFilter.Pools.Inc1.Get(e);
                var modelInstance = disabledFilter.Pools.Inc2.Get(e);
                
                if (modelInstance.modelGameObject && model.isModelActive)
                {
                    modelInstance.modelGameObject.SetActive(false);
                    model.isModelActive = false;
                }
                
                disabledFilter.Pools.Inc3.Del(e);
                // world.RemoveComponent<ModelEnabledComponent>(e);
            }
            
            // Profiler.EndSample();
        }

        private void CreateModel(int entity, ref ModelComponent model)
        {
            if (!model.prefab)
            {
                return;
            }

            // if (world.HasComponent<ModelInstanceComponent>(entity))
            // {
            //     return;
            // }

            var modelInstance = new ModelInstanceComponent();
            
            modelInstance.modelGameObject = poolMap.Get(model.prefab);

            if (!modelInstance.modelGameObject.HasComponent<EntityReference>())
            {
                modelInstance.modelGameObject.AddComponent<EntityReference>();
            }
                    
            var entityReference = modelInstance.modelGameObject.GetComponent<EntityReference>();
            entityReference.entity = world.GetEntity(entity);

            modelInstance.instance = modelInstance.modelGameObject.GetComponent<Model>();
            modelInstance.hasSubModelObject = modelInstance.instance.model;
            
            if (modelInstance.instance.sortingGroup)
            {
                world.AddComponent(entity, new ModelSortingGroupComponent()
                {
                    sortingGroup = modelInstance.instance.sortingGroup,
                    layer = model.sortingLayer,
                    order = model.sortingOrder,
                    updated = false
                });
            }
            
            modelInstance.modelGameObject.SetActive(true);
            model.isModelActive = true;
            
            world.AddComponent(entity, new ModelEnabledComponent());
            world.AddComponent(entity, modelInstance);
        }
        
        public void OnEntityCreated(World world, Entity entity)
        {
            var modelComponents = world.GetComponents<ModelComponent>();
            var modelInstanceComponents = world.GetComponents<ModelInstanceComponent>();
            if (modelComponents.Has(entity) && !modelInstanceComponents.Has(entity))
            {
                ref var model = ref modelComponents.Get(entity);
                CreateModel(entity, ref model);
            }
        }

        public void OnEntityDestroyed(World world, Entity entity)
        {
            var modelInstanceComponents = world.GetComponents<ModelInstanceComponent>();
            if (modelInstanceComponents.Has(entity))
            {
                ref var model = ref modelInstanceComponents.Get(entity);
                if (model.instance)
                {
                    model.instance.ResetModel();
                    poolMap.Release(model.modelGameObject);
                }
                model.instance = null;
                model.modelGameObject = null;
            }
        }


    }
}