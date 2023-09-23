using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MyBox;
using UnityEngine;

namespace Game.Systems
{
    public class ModelUpdateSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<ModelComponent>, Exc<DisabledComponent>> modelFilter = default;
        readonly EcsFilterInject<Inc<ModelComponent, PositionComponent>, Exc<DisabledComponent>> positionFilter = default;
        
        public void Run(EcsSystems systems)
        {
            var modelComponents = world.GetComponents<ModelComponent>();
            var interpolationComponents = world.GetComponents<ModelInterpolationComponent>();
            
            var positionComponents = world.GetComponents<PositionComponent>();
            var lookingDirectionComponents = world.GetComponents<LookingDirection>();

            foreach (var entity in modelFilter.Value)
            {
                var modelComponent = modelFilter.Pools.Inc1.Get(entity);

                var model = modelComponent.instance;
                
                if (!modelComponent.instance.isActiveAndEnabled)
                {
                    modelComponent.instance.gameObject.SetActive(true);
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

                if (model.gameObject.activeSelf && !modelComponent.IsVisible)
                {
                    model.gameObject.SetActive(false);
                } else if (!model.gameObject.activeSelf && modelComponent.IsVisible)
                {
                    model.gameObject.SetActive(true);
                }
            }
            
            foreach (var entity in positionFilter.Value)
            {
                ref var modelComponent = ref positionFilter.Pools.Inc1.Get(entity);
                var positionComponent = positionFilter.Pools.Inc2.Get(entity);

                if (positionComponent.type == 0)
                {
                    var position = GamePerspective.ConvertFromWorld(positionComponent.value);
                    
                    if (modelComponent.hasSubModelObject)
                    {
                        modelComponent.instance.transform.position = new Vector3(position.x, position.y, 0);
                        modelComponent.instance.model.localPosition = new Vector3(0, position.z, 0);
                    }
                    else
                    {
                        modelComponent.instance.transform.position =
                            new Vector3(position.x, position.y + position.z, 0);
                    }
                }
                else if (positionComponent.type == 1)
                {
                    var position = positionComponent.value;
                    
                    if (modelComponent.hasSubModelObject)
                    {
                        modelComponent.instance.transform.position = new Vector3(position.x, 0, position.z);
                        modelComponent.instance.model.localPosition = new Vector3(0, position.y, 0);
                    }
                    else
                    {
                        modelComponent.instance.transform.position = position;
                    }
                }
            }
            
            foreach (var entity in world.GetFilter<ModelComponent>()
                         .Inc<ModelInterpolationComponent>()
                         .Exc<DisabledComponent>()
                         .End())
            {
                ref var modelComponent = ref modelComponents.Get(entity);
                var interpolationComponent = interpolationComponents.Get(entity);

                var position = interpolationComponent.position;
                
                if (modelComponent.hasSubModelObject)
                {
                    modelComponent.instance.transform.position = new Vector3(position.x, position.y, 0);
                    modelComponent.instance.model.localPosition = new Vector3(0, position.z, 0);
                }
                else
                {
                    modelComponent.instance.transform.position = new Vector3(position.x, position.y + position.z, 0);
                }
            }

            foreach (var entity in world.GetFilter<ModelComponent>()
                         .Inc<PositionComponent>()
                         .Inc<LookingDirection>()
                         .Exc<DisabledComponent>()
                         .End())
            {
                var modelComponent = modelComponents.Get(entity);
                var lookingDirection = lookingDirectionComponents.Get(entity);
                var positionComponent = positionComponents.Get(entity);
                
                var modelInstance = modelComponent.instance;

                var scale = modelInstance.transform.localScale;

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

                        var t = modelInstance.transform;

                        t.localScale = scale;

                        modelComponent.instance.model.localEulerAngles = Vector3.zero;
                    }
                    else
                    {
                        if (shouldFlip)
                        {
                            modelInstance.transform.localEulerAngles = new Vector3(0, flip ? 180 : 0, 0);
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

                    var t = objectModel.spriteRenderer.transform;
                    
                    t.localEulerAngles = t.localEulerAngles.SetZ(angle);
                    var modelScale = t.localScale;
                    t.localScale = new Vector3(direction2d.magnitude, modelScale.y, modelScale.z);
                }
            }

            foreach (var entity in world.GetFilter<ModelComponent>()
                         .Inc<DisabledComponent>()
                         .End())
            {
                var modelComponent = modelComponents.Get(entity);
                if (modelComponent.instance != null)
                {
                    if (modelComponent.instance.isActiveAndEnabled)
                    {
                        modelComponent.instance.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}