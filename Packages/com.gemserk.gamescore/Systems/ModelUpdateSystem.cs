using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MyBox;
using UnityEngine;

namespace Game.Systems
{
    public class ModelUpdateSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<ModelComponent, ModelInstanceComponent>, Exc<DisabledComponent>> modelFilter = default;
        
        readonly EcsFilterInject<Inc<ModelComponent, ModelInstanceComponent>, Exc<DisabledComponent, ModelSortingGroupComponent>> modelWithoutSortingFilter = default;
        readonly EcsFilterInject<Inc<ModelComponent, ModelSortingGroupComponent>, Exc<DisabledComponent>> modelWithSortingFilter = default;
        
        readonly EcsFilterInject<Inc<ModelInstanceComponent, PositionComponent>, Exc<DisabledComponent, ModelStaticProcessedComponent>> positionFilter = default;
        
        readonly EcsFilterInject<Inc<PositionComponent, LookingDirection>, Exc<DisabledComponent>> 
            lookingDirectionAngleFilter = default;
        
        readonly EcsFilterInject<Inc<ModelComponent, ModelInstanceComponent, LookingDirection>, Exc<DisabledComponent>> 
            modelAllFilter = default;
        
        readonly EcsFilterInject<Inc<ModelInstanceComponent, ModelInterpolationComponent>, Exc<DisabledComponent, ModelStaticProcessedComponent>> 
            modelInterpolationFilter = default;
        
        readonly EcsFilterInject<Inc<ModelComponent, ModelInstanceComponent, DisabledComponent>> 
            disabledFilter = default;
        
        readonly EcsFilterInject<Inc<StaticObjectComponent>, Exc< DisabledComponent, ModelStaticProcessedComponent>> 
            staticModels = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in modelFilter.Value)
            {
                ref var modelComponent = ref modelFilter.Pools.Inc1.Get(entity);
                ref var modelInstance = ref modelFilter.Pools.Inc2.Get(entity);
                
                if (!modelComponent.isModelActive && modelComponent.IsVisible)
                {
                    modelInstance.modelGameObject.SetActive(true);
                    modelComponent.isModelActive = true;
                } else if (modelComponent.isModelActive && !modelComponent.IsVisible)
                {
                    modelInstance.modelGameObject.SetActive(false);
                    modelComponent.isModelActive = false;
                    continue;
                }
                
                if (modelInstance.instance.hasSpriteRenderer)
                {
                    modelInstance.instance.spriteRenderer.color = modelComponent.color;
                }
            }
            
            foreach (var entity in modelWithoutSortingFilter.Value)
            {
                ref var modelComponent = ref modelWithoutSortingFilter.Pools.Inc1.Get(entity);
                ref var modelInstance = ref modelWithoutSortingFilter.Pools.Inc2.Get(entity);
                
                // For now will assume we don't want to update sorting all the time.
                if (modelComponent.sortingUpdated)
                {
                    continue;
                }
                
                if (modelComponent.sortingLayerType == ModelComponent.SortingLayerType.CopyFromComponent)
                {
                    var model = modelInstance.instance;
                    model.spriteRenderer.sortingOrder = modelComponent.sortingOrder;
                    model.spriteRenderer.sortingLayerID = modelComponent.sortingLayer;
                    modelComponent.sortingUpdated = true;
                }
            }
            
            foreach (var entity in modelWithSortingFilter.Value)
            {
                var modelComponent = modelWithSortingFilter.Pools.Inc1.Get(entity);
                ref var modelSortingGroupComponent = ref modelWithSortingFilter.Pools.Inc2.Get(entity);
                
                if (modelSortingGroupComponent.updated)
                {
                    continue;
                }
                
                if (modelComponent.sortingLayerType == ModelComponent.SortingLayerType.CopyFromComponent)
                {
                    // For now will assume we don't want to update sorting all the time.
                    modelSortingGroupComponent.sortingGroup.sortingOrder = modelSortingGroupComponent.order;
                    modelSortingGroupComponent.sortingGroup.sortingLayerID = modelSortingGroupComponent.layer;
                    modelSortingGroupComponent.updated = true;
                }
            }
            
            foreach (var entity in positionFilter.Value)
            {
                ref var modelInstance = ref positionFilter.Pools.Inc1.Get(entity);
                var positionComponent = positionFilter.Pools.Inc2.Get(entity);

                if (positionComponent.type == 0)
                {
                    var position = GamePerspective.ConvertFromWorld(positionComponent.value);
                    
                    if (modelInstance.hasSubModelObject)
                    {
                        modelInstance.instance.cachedTransform.position = new Vector3(position.x, position.y, 0);
                        modelInstance.instance.model.localPosition = new Vector3(0, position.z, 0);
                    }
                    else
                    {
                        modelInstance.instance.cachedTransform.position =
                            new Vector3(position.x, position.y + position.z, 0);
                    }
                }
                else if (positionComponent.type == 1)
                {
                    var position = positionComponent.value;
                    
                    if (modelInstance.hasSubModelObject)
                    {
                        modelInstance.instance.cachedTransform.position = new Vector3(position.x, 0, position.z);
                        modelInstance.instance.model.localPosition = new Vector3(0, position.y, 0);
                    }
                    else
                    {
                        modelInstance.instance.cachedTransform.position = position;
                    }
                }
            }

            foreach (var entity in modelInterpolationFilter.Value)
            {
                ref var modelInstance = ref modelInterpolationFilter.Pools.Inc1.Get(entity);
                var interpolationComponent = modelInterpolationFilter.Pools.Inc2.Get(entity);

                var position = interpolationComponent.position;
                
                if (modelInstance.hasSubModelObject)
                {
                    modelInstance.instance.cachedTransform.position = new Vector3(position.x, position.y, 0);
                    modelInstance.instance.model.localPosition = new Vector3(0, position.z, 0);
                }
                else
                {
                    modelInstance.instance.cachedTransform.position = new Vector3(position.x, position.y + position.z, 0);
                }
            }
            
            foreach (var entity in lookingDirectionAngleFilter.Value)
            {
                var position = lookingDirectionAngleFilter.Pools.Inc1.Get(entity);
                ref var lookingDirection = ref lookingDirectionAngleFilter.Pools.Inc2.Get(entity);
                
                var direction = GamePerspective.ProjectFromWorld(lookingDirection.value);

                var p0 = GamePerspective.ProjectFromWorld(position.value);
                var p1 = p0 + direction;

                var angle = Vector2.SignedAngle(Vector2.right, p1 - p0);

                lookingDirection.angle = angle;
            }

            foreach (var entity in modelAllFilter.Value)
            {
                var modelComponent = modelAllFilter.Pools.Inc1.Get(entity);
                ref var modelInstance = ref modelAllFilter.Pools.Inc2.Get(entity);
                var lookingDirection = modelAllFilter.Pools.Inc3.Get(entity);
                
                var modelTransform = modelInstance.instance.cachedTransform;

                var scale = modelTransform.localScale;

                if (modelComponent.rotation == ModelComponent.RotationType.FlipToLookingDirection)
                {
                    var shouldFlip = Mathf.Abs(lookingDirection.value.x) > 0;
                    
                    var flip = false;
                    
                    if (shouldFlip)
                    {
                        flip = lookingDirection.value.x < 0;
                    }
                    
                    if (!modelComponent.flipUseRotation)
                    {
                        if (shouldFlip)
                        {
                            scale.x = flip ? -1 : 1;
                        }
                        
                        modelTransform.localScale = scale;

                        modelInstance.instance.model.localEulerAngles = Vector3.zero;
                    }
                    else
                    {
                        if (shouldFlip)
                        {
                            modelTransform.localEulerAngles = new Vector3(0, flip ? 180 : 0, 0);
                        }
                    }
                }
                else if (modelComponent.rotation == ModelComponent.RotationType.Rotate)
                {
                    var direction = GamePerspective.ProjectFromWorld(lookingDirection.value);
                    
                    var angle = lookingDirection.angle;
                    
                    var objectModel = modelInstance.instance;

                    var t = objectModel.model;

                    if (modelComponent.fixedRotationAngles > 0)
                    {
                        var partitions = 360 / modelComponent.fixedRotationAngles;
                        angle = Mathf.Round(partitions * (angle / 360.0f)) * modelComponent.fixedRotationAngles;
                    }
                    
                    t.localEulerAngles = t.localEulerAngles.SetZ(angle);
                    var modelScale = t.localScale;
                    t.localScale = new Vector3(direction.magnitude, modelScale.y, modelScale.z);
                }
            }
            
            foreach (var e in staticModels.Value)
            {
                world.AddComponent(e, new ModelStaticProcessedComponent());
            }

            foreach (var entity in disabledFilter.Value)
            {
                var modelComponent = disabledFilter.Pools.Inc1.Get(entity);
                var modelInstance = disabledFilter.Pools.Inc2.Get(entity);
                
                if (modelInstance.modelGameObject)
                {
                    if (modelComponent.isModelActive)
                    {
                        modelInstance.modelGameObject.SetActive(false);
                        modelComponent.isModelActive = false;
                    }
                }
            }
        }
    }
}