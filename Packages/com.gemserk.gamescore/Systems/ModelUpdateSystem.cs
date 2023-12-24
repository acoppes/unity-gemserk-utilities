using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MyBox;
using UnityEngine;
using UnityEngine.Profiling;

namespace Game.Systems
{
    public class ModelUpdateSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<ModelComponent>, Exc<DisabledComponent>> modelFilter = default;
        readonly EcsFilterInject<Inc<ModelComponent, PositionComponent>, Exc<DisabledComponent>> positionFilter = default;
        
        readonly EcsFilterInject<Inc<ModelComponent, PositionComponent, LookingDirection>, Exc<DisabledComponent>> 
            modelAllFilter = default;
        
        readonly EcsFilterInject<Inc<ModelComponent, ModelInterpolationComponent>, Exc<DisabledComponent>> 
            modelInterpolationFilter = default;
        
        readonly EcsFilterInject<Inc<ModelComponent, DisabledComponent>> 
            disabledFilter = default;
        
        public void Run(EcsSystems systems)
        {
            Profiler.BeginSample("Update1");
            foreach (var entity in modelFilter.Value)
            {
                ref var modelComponent = ref modelFilter.Pools.Inc1.Get(entity);

                var model = modelComponent.instance;
                
                if (!modelComponent.isModelActive && modelComponent.IsVisible)
                {
                    modelComponent.modelGameObject.SetActive(true);
                    modelComponent.isModelActive = true;
                } else if (modelComponent.isModelActive && !modelComponent.IsVisible)
                {
                    modelComponent.modelGameObject.SetActive(false);
                    modelComponent.isModelActive = false;
                    continue;
                }
                
                if (model.spriteRenderer != null)
                {
                    model.spriteRenderer.color = modelComponent.color;
                }
                
                if (modelComponent.sortingLayerType == ModelComponent.SortingLayerType.CopyFromComponent)
                {
                    if (model.sortingGroup != null)
                    {
                        model.sortingGroup.sortingOrder = modelComponent.sortingOrder;
                        model.sortingGroup.sortingLayerID = modelComponent.sortingLayer;
                    } else if (model.spriteRenderer != null)
                    {
                        model.spriteRenderer.sortingOrder = modelComponent.sortingOrder;
                        model.spriteRenderer.sortingLayerID = modelComponent.sortingLayer;
                    }
                }

                // if (!modelComponent.IsVisible)
                // {
                //     modelComponent.modelGameObject.SetActive(false);
                //     modelComponent.isModelActive = false;
                // } else if (modelComponent.IsVisible)
                // {
                //     modelComponent.modelGameObject.SetActive(true);
                //     modelComponent.isModelActive = true;
                // }
            }
            Profiler.EndSample();
            
            Profiler.BeginSample("Update2");
            foreach (var entity in positionFilter.Value)
            {
                ref var modelComponent = ref positionFilter.Pools.Inc1.Get(entity);
                var positionComponent = positionFilter.Pools.Inc2.Get(entity);

                if (positionComponent.type == 0)
                {
                    var position = GamePerspective.ConvertFromWorld(positionComponent.value);
                    
                    if (modelComponent.hasSubModelObject)
                    {
                        modelComponent.instance.cachedTransform.position = new Vector3(position.x, position.y, 0);
                        modelComponent.instance.model.localPosition = new Vector3(0, position.z, 0);
                    }
                    else
                    {
                        modelComponent.instance.cachedTransform.position =
                            new Vector3(position.x, position.y + position.z, 0);
                    }
                }
                else if (positionComponent.type == 1)
                {
                    var position = positionComponent.value;
                    
                    if (modelComponent.hasSubModelObject)
                    {
                        modelComponent.instance.cachedTransform.position = new Vector3(position.x, 0, position.z);
                        modelComponent.instance.model.localPosition = new Vector3(0, position.y, 0);
                    }
                    else
                    {
                        modelComponent.instance.cachedTransform.position = position;
                    }
                }
            }
            Profiler.EndSample();
            
            Profiler.BeginSample("Update3");
            foreach (var entity in modelInterpolationFilter.Value)
            {
                ref var modelComponent = ref modelInterpolationFilter.Pools.Inc1.Get(entity);
                var interpolationComponent = modelInterpolationFilter.Pools.Inc2.Get(entity);

                var position = interpolationComponent.position;
                
                if (modelComponent.hasSubModelObject)
                {
                    modelComponent.instance.cachedTransform.position = new Vector3(position.x, position.y, 0);
                    modelComponent.instance.model.localPosition = new Vector3(0, position.z, 0);
                }
                else
                {
                    modelComponent.instance.cachedTransform.position = new Vector3(position.x, position.y + position.z, 0);
                }
            }
            Profiler.EndSample();

            Profiler.BeginSample("Update4");
            foreach (var entity in modelAllFilter.Value)
            {
                var modelComponent = modelAllFilter.Pools.Inc1.Get(entity);
                var positionComponent = modelAllFilter.Pools.Inc2.Get(entity);
                var lookingDirection = modelAllFilter.Pools.Inc3.Get(entity);
                
                var modelInstance = modelComponent.instance;

                var modelTransform = modelInstance.cachedTransform;

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

                        modelComponent.instance.model.localEulerAngles = Vector3.zero;
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
                    var direction3d = lookingDirection.value;
                    var direction2d = GamePerspective.ProjectFromWorld(direction3d);

                    var p0 = GamePerspective.ProjectFromWorld(positionComponent.value);
                    var p1 = p0 + direction2d;

                    var angle = Vector2.SignedAngle(Vector2.right, p1 - p0);
                    
                    var objectModel = modelComponent.instance;

                    var t = objectModel.model;

                    if (modelComponent.fixedRotationAngles > 0)
                    {
                        var partitions = 360 / modelComponent.fixedRotationAngles;
                        angle = Mathf.Round(partitions * (angle / 360.0f)) * modelComponent.fixedRotationAngles;
                    }
                    
                    t.localEulerAngles = t.localEulerAngles.SetZ(angle);
                    var modelScale = t.localScale;
                    t.localScale = new Vector3(direction2d.magnitude, modelScale.y, modelScale.z);
                }
            }
            Profiler.EndSample();
            
            Profiler.BeginSample("Update5");
            foreach (var entity in disabledFilter.Value)
            {
                var modelComponent = disabledFilter.Pools.Inc1.Get(entity);
                if (modelComponent.modelGameObject != null)
                {
                    if (modelComponent.isModelActive)
                    {
                        modelComponent.modelGameObject.SetActive(false);
                        modelComponent.isModelActive = false;
                    }
                }
            }
            Profiler.EndSample();
        }
    }
}