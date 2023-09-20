using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities.Pooling;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class LookingDirectionIndicatorSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler, IEntityDestroyedHandler,
        IEcsInitSystem
    {
        readonly EcsFilterInject<Inc<HasLookingDirectionIndicatorComponent>, Exc<DisabledComponent>> visibilityFilter = default;
        readonly EcsFilterInject<Inc<HasLookingDirectionIndicatorComponent, LookingDirection, PositionComponent>, Exc<DisabledComponent>> filter = default;
        readonly EcsFilterInject<Inc<HasLookingDirectionIndicatorComponent, PositionComponent>, Exc<DisabledComponent>> positionsFilter = default;
        
        [SerializeField]
        protected GameObject indicatorPrefab;

        private GameObjectPool pool;
        
        public void Init(EcsSystems systems)
        {
            pool = new GameObjectPool(indicatorPrefab, "~LookingDirectionIndicator");
        }
        
        public void OnEntityCreated(World world, Entity entity)
        {
            var lookingDirectionIndicators = world.GetComponents<HasLookingDirectionIndicatorComponent>();
            if (lookingDirectionIndicators.Has(entity))
            {
                ref var indicatorComponent = ref lookingDirectionIndicators.Get(entity);
                indicatorComponent.instance = pool.Get();
                indicatorComponent.pivot = indicatorComponent.instance.transform.Find("Pivot");
            }
        }

        public void OnEntityDestroyed(World world, Entity entity)
        {
            var indicators = world.GetComponents<HasLookingDirectionIndicatorComponent>();
            if (indicators.Has(entity))
            {
                ref var indicatorComponent = ref indicators.Get(entity);
                if (indicatorComponent.instance != null)
                {
                    pool.Release(indicatorComponent.instance);
                }
                indicatorComponent.instance = null;
            }
        }
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in visibilityFilter.Value)
            {
                ref var indicator = ref visibilityFilter.Pools.Inc1.Get(entity);
                if (indicator.instance != null)
                {
                    if (indicator.visiblity == ModelComponent.Visiblity.Hidden && indicator.instance.activeSelf)
                    {
                        indicator.instance.SetActive(false);
                    } else if (indicator.visiblity == ModelComponent.Visiblity.Visible &&
                               !indicator.instance.activeSelf)
                    {
                        indicator.instance.SetActive(true);
                    }
                }
            }
            
            foreach (var entity in filter.Value)
            {
                var indicator = filter.Pools.Inc1.Get(entity);
                var lookingDirection = filter.Pools.Inc2.Get(entity);
                var position = filter.Pools.Inc3.Get(entity);
                
                var pivot = indicator.pivot;

                var eulerAngles = pivot.localEulerAngles;
                
                if (position.type == 0)
                {
                    var lookingDirection2d = GamePerspective.ProjectFromWorld(lookingDirection.value);
                    eulerAngles.z = Vector2.SignedAngle(Vector2.right, lookingDirection2d);
                }
                else if (position.type == 1) 
                {
                    eulerAngles.z = Vector2.SignedAngle(Vector2.right, lookingDirection.value);
                }
                
                pivot.localEulerAngles = eulerAngles;
            }
            
            foreach (var entity in positionsFilter.Value)
            {
                var indicator = positionsFilter.Pools.Inc1.Get(entity);
                var position = positionsFilter.Pools.Inc2.Get(entity);
                
                var indicatorInstance = indicator.instance;
                
                if (position.type == 0)
                {
                    indicatorInstance.transform.position = GamePerspective.ConvertFromWorld(position.value);
                } else if (position.type == 1)
                {
                    indicatorInstance.transform.position = position.value;
                }
            }
        }
    }
}