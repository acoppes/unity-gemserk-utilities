using Game.Components;
using Game.Definitions;
using Game.Models;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using MyBox;
using UnityEngine;
using Vertx.Debugging;

namespace Game.LevelDesign
{
    [ExecuteAlways]
    [SelectionBase]
    public class ObjectDefinitionPreview : MonoBehaviour
    {
        public bool disablePreview;
        public bool showName;
        public bool showComponentsPreview;

#if UNITY_EDITOR

        private static string PreviewObjectName = "~PreviewObject";
        
        private GameObject previewObject;

        private void OnDestroy()
        {
            if (previewObject != null)
            {
                GameObject.DestroyImmediate(previewObject);
            }
            previewObject = null;
        }

        private void Awake()
        {
            var previewObjectTransform = transform.Find(PreviewObjectName);
            if (previewObjectTransform != null)
            {
                previewObject = previewObjectTransform.gameObject;
            }
        }

        public void Update()
        {
            if (Application.isPlaying)
            {
                return;
            }

            if (previewObject == null)
            {
                var previewObjectTransform = transform.Find(PreviewObjectName);
                if (previewObjectTransform != null)
                {
                    previewObject = previewObjectTransform.gameObject;
                }
            }

            if (previewObject != null)
            {
                UpdatePreview();
                return;
            }

            RegeneratePreview();
        }

        private void UpdatePreview()
        {
            previewObject.transform.SetParent(transform);
            previewObject.transform.localPosition = Vector3.zero;

            var entityPrefabInstance = GetComponent<EntityPrefabInstance>();

            if (entityPrefabInstance.entityDefinition == null)
            {
                return;
            }

            var objectDefinition = entityPrefabInstance.entityDefinition.GetInterface<ObjectEntityDefinition>();

            if (objectDefinition == null)
            {
                return;
            }

            var unitInstanceParameter = GetComponent<UnitInstanceParameter>();
            var positionType = UnitInstanceParameter.PositionType.CopyFromTransform;

            if (unitInstanceParameter != null)
            {
                positionType = unitInstanceParameter.positionType;
            }
            

            var modelComponentDefinition = objectDefinition.GetComponent<ModelComponentDefinition>();
            var animationComponentDefinition = objectDefinition.GetComponent<AnimationComponentDefinition>();
            
            if (modelComponentDefinition != null && animationComponentDefinition != null 
                                                 && animationComponentDefinition.animationsAsset != null)
            {
                if (modelComponentDefinition.prefab != null)
                {
                    var model = previewObject.GetComponent<Model>();
                    var defaultAnim = 0;

                    var defaultAnimation = string.IsNullOrEmpty(animationComponentDefinition.defaultAnimation)
                        ? "None"
                        : animationComponentDefinition.defaultAnimation;
                    
                    if (!string.IsNullOrEmpty(defaultAnimation))
                    {
                        defaultAnim =
                            animationComponentDefinition.animationsAsset.GetAnimationIndexByName(defaultAnimation);
                    }

                    if (defaultAnim != -1)
                    {
                        model.spriteRenderer.sprite = animationComponentDefinition.animationsAsset.animations[defaultAnim].frames[0].sprite;
                        
                        if (positionType == UnitInstanceParameter.PositionType.CopyFromTransform)
                        {
                            model.spriteRenderer.transform.localPosition = new Vector3(0, transform.position.z, 0);
                        } else if (positionType == UnitInstanceParameter.PositionType.CopyFromTransformDontConvert)
                        {
                            model.spriteRenderer.transform.localPosition = new Vector3(0, 0, 0);
                        }
                    }

                    if (modelComponentDefinition.sortingLayerType ==
                        ModelComponent.SortingLayerType.CopyFromComponent)
                    {
                        if (model.sortingGroup != null)
                        {
                            model.sortingGroup.sortingLayerName = modelComponentDefinition.sortingLayer;
                            model.sortingGroup.sortingOrder = modelComponentDefinition.sortingOrder;
                        }
                        else if (model.spriteRenderer != null)
                        {
                            model.spriteRenderer.sortingOrder =  modelComponentDefinition.sortingOrder;
                            model.spriteRenderer.sortingLayerName =  modelComponentDefinition.sortingLayer;
                        }
                    }
                }
            }
        }

        [ButtonMethod()]
        public void RegeneratePreview()
        {
            var previewObjectTransform = transform.Find(PreviewObjectName);
            if (previewObjectTransform != null)
            {
                GameObject.DestroyImmediate(previewObjectTransform.gameObject);
                previewObject = null;
            }

            if (disablePreview)
            {
                return;
            }
            
            var entityPrefabInstance = GetComponent<EntityPrefabInstance>();

            if (entityPrefabInstance == null)
            {
                return;
            }

            if (entityPrefabInstance.entityDefinition == null)
            {
                return;
            }

            var objectDefinition = entityPrefabInstance.entityDefinition.GetInterface<ObjectEntityDefinition>();

            if (objectDefinition == null)
            {
                return;
            }
            
            var modelComponentDefinition = objectDefinition.GetComponent<ModelComponentDefinition>();
           
            if (previewObject == null && modelComponentDefinition != null && modelComponentDefinition.prefab != null)
            {
                previewObject = GameObject.Instantiate(modelComponentDefinition.prefab, transform, true);
                previewObject.SetActive(true);
                
                previewObject.name = PreviewObjectName;
                previewObject.AddComponent<PreviewObject>();
                // previewObject.hideFlags = HideFlags.DontSave | HideFlags.HideInHierarchy;
                previewObject.hideFlags = HideFlags.DontSave;
                
                UpdatePreview();
            }
        }
        
        private void OnDrawGizmos()
        {
            if (!disablePreview && showName)
            {
                var parameter = GetComponent<UnitInstanceParameter>();
                if (parameter != null)
                {
                    if (parameter.namingType == UnitInstanceParameter.NamingType.String)
                    {
                        D.raw(new Shape.Text(transform.position, parameter.entityName));
                    }
                }
            }
            
            if (showComponentsPreview)
            {
                var entityPrefabInstance = GetComponent<EntityPrefabInstance>();

                if (entityPrefabInstance.entityDefinition == null)
                {
                    return;
                }
                
                var objectEntityDefinition = entityPrefabInstance.entityDefinition.GetInterface<ObjectEntityDefinition>();
            
                if (objectEntityDefinition == null)
                {
                    return;
                }
                
                var componentPreviews = objectEntityDefinition.GetComponents<IComponentPreview>();
                foreach (var componentPreview in componentPreviews)
                {
                    componentPreview.DrawGizmos(transform.position);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            var entityPrefabInstance = GetComponent<EntityPrefabInstance>();

            if (entityPrefabInstance.entityDefinition == null)
            {
                return;
            }

            var objectEntityDefinition = entityPrefabInstance.entityDefinition.GetInterface<ObjectEntityDefinition>();
            
            if (objectEntityDefinition == null)
            {
                return;
            }

            var physicsComponentDefinition = objectEntityDefinition.GetComponent<PhysicsComponentDefinition>();

            if (physicsComponentDefinition == null || physicsComponentDefinition.prefab != null)
            {
                return;
            }

            var center = physicsComponentDefinition.center;

            if (physicsComponentDefinition.shapeType == PhysicsComponent.ShapeType.Circle)
            {
                // var size = GamePerspective.ConvertFromWorld()
                D.raw(new Shape.Sphere(transform.position + center, physicsComponentDefinition.size.x), 
                    Color.green);
            }
            
            if (physicsComponentDefinition.shapeType == PhysicsComponent.ShapeType.Box)
            {
                var extents = physicsComponentDefinition.size;
                // var extents = GamePerspective.ConvertFromWorld(physicsComponentDefinition.size);
                D.raw(new Shape.Box(transform.position + center, 
                    extents * 0.5f), Color.green);
            }
            
            if (physicsComponentDefinition.shapeType == PhysicsComponent.ShapeType.Capsule)
            {
                D.raw(new Shape.Capsule(transform.position, Vector3.forward, 
                    physicsComponentDefinition.size.y, physicsComponentDefinition.size.x), Color.green);
                D.raw(new Shape.Line(transform.position, transform.position + new Vector3(0, physicsComponentDefinition.size.y, 0)), 
                    Color.green);
            }
        }
#endif
    }
}