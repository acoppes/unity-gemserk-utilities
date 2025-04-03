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
            // Profiler.BeginSample("ModelSystem");
            
            foreach (var entity in filter.Value)
            {
                ref var model = ref filter.Pools.Inc1.Get(entity);
                CreateModel(entity, ref model);
            }
            
            foreach (var e in disabledFilter.Value)
            {
                ref var model = ref disabledFilter.Pools.Inc1.Get(e);
                
                if (model.modelGameObject != null && model.isModelActive)
                {
                    model.modelGameObject.SetActive(false);
                    model.isModelActive = false;
                }
                
                disabledFilter.Pools.Inc2.Del(e);
                // world.RemoveComponent<ModelEnabledComponent>(e);
            }
            
            // Profiler.EndSample();
        }

        private void CreateModel(int entity, ref ModelComponent model)
        {
            if (model.prefab != null && model.modelGameObject == null)
            {
                model.modelGameObject = poolMap.Get(model.prefab);

                if (!model.modelGameObject.HasComponent<EntityReference>())
                {
                    model.modelGameObject.AddComponent<EntityReference>();
                }
                    
                var entityReference = model.modelGameObject.GetComponent<EntityReference>();
                entityReference.entity = world.GetEntity(entity);

                model.instance = model.modelGameObject.GetComponent<Model>();
                model.hasSubModelObject = model.instance.model != null;
                
                model.modelGameObject.SetActive(true);
                model.isModelActive = true;
                
                world.AddComponent(entity, new ModelEnabledComponent());

                if (model.instance.sortingGroup != null)
                {
                    world.AddComponent(entity, new ModelSortingGroupComponent()
                    {
                        sortingGroup = model.instance.sortingGroup,
                        layer = model.sortingLayer,
                        order = model.sortingOrder,
                        updated = false
                    });
                }
            }
        }
        
        public void OnEntityCreated(World world, Entity entity)
        {
            var modelComponents = world.GetComponents<ModelComponent>();
            if (modelComponents.Has(entity))
            {
                ref var model = ref modelComponents.Get(entity);
                CreateModel(entity, ref model);
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
                    poolMap.Release(model.modelGameObject);
                }
                model.instance = null;
                model.modelGameObject = null;
                model.isModelActive = false;
            }
        }


    }
}