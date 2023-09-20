using Game.Components;
using Game.Models;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities.Pooling;
using Leopotam.EcsLite;
using MyBox;
using UnityEngine;

namespace Game.Systems
{
    public class ShadowModelSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler, IEntityDestroyedHandler, IEcsInitSystem
    {
        public GameObject shadowPrefab;
        
        public float baseShadowOpacity = 0.4f;

        private GameObjectPoolMap poolMap;
        
        public void Init(EcsSystems systems)
        {
            poolMap = new GameObjectPoolMap("~ShadowModels");
        }
        
        public void OnEntityCreated(World world, Entity entity)
        {
            // create model if model component
            var hasShadowComponents = world.GetComponents<HasShadowComponent>();
            if (hasShadowComponents.Has(entity))
            {
                ref var hasShadowComponent = ref hasShadowComponents.Get(entity);

                if (hasShadowComponent.instance == null && hasShadowComponent.shadowDefintion == null)
                {
                    if (hasShadowComponent.prefab == null)
                    {
                        hasShadowComponent.prefab = shadowPrefab;
                    }

                    var modelInstance = poolMap.Get(hasShadowComponent.prefab);

                    if (!modelInstance.HasComponent<EntityReference>())
                    {
                        modelInstance.AddComponent<EntityReference>();
                    }
                    
                    var entityReference = modelInstance.GetComponent<EntityReference>();
                    entityReference.entity = entity;
                
                    hasShadowComponent.instance = modelInstance.GetComponent<Shadow>();
                }
            }
        }

        public void OnEntityDestroyed(World world, Entity entity)
        {
            // destroy model if model component
            var hasShadowComponents = world.GetComponents<HasShadowComponent>();
            if (hasShadowComponents.Has(entity))
            {
                ref var hasShadowComponent = ref hasShadowComponents.Get(entity);
                if (hasShadowComponent.instance != null)
                {
                    hasShadowComponent.instance.ResetModel();
                    poolMap.Release(hasShadowComponent.instance.gameObject);
                }
                hasShadowComponent.instance = null;
            }
        }

        public void Run(EcsSystems systems)
        {
            var hasShadowComponents = world.GetComponents<HasShadowComponent>();
            var shadowComponents = world.GetComponents<ShadowComponent>();
            var positionComponents = world.GetComponents<PositionComponent>();
            
            var modelComponents = world.GetComponents<ModelComponent>();
            var lookingDirectionComponents = world.GetComponents<LookingDirection>();
            
            foreach (var entity in world.GetFilter<HasShadowComponent>()
                         .Inc<PositionComponent>()
                         .Exc<DisabledComponent>()
                         .End())
            {
                ref var hasShadowComponent = ref hasShadowComponents.Get(entity);
                var positionComponent = positionComponents.Get(entity);
                
                if (hasShadowComponent.copyFromPositionType == HasShadowComponent.CopyFromPositionType.FromFake3d)
                {
                    hasShadowComponent.position = positionComponent.value;
                    hasShadowComponent.position.y = 0;
                    hasShadowComponent.position =
                        GamePerspective.ConvertFromWorld(hasShadowComponent.position);
                } else if (hasShadowComponent.copyFromPositionType == HasShadowComponent.CopyFromPositionType.From2d)
                {
                    hasShadowComponent.position = positionComponent.value;
                } else if (hasShadowComponent.copyFromPositionType == HasShadowComponent.CopyFromPositionType.From3d)
                {
                    // project it to ground instead?
                    hasShadowComponent.position = positionComponent.value.SetY(0);
                }
            }
            
            foreach (var entity in world.GetFilter<HasShadowComponent>()
                         .Exc<DisabledComponent>()
                         .End())
            {
                var hasShadowComponent = hasShadowComponents.Get(entity);

                if (hasShadowComponent.instance == null)
                {
                    continue;
                }

                hasShadowComponent.instance.transform.position =
                    hasShadowComponent.position;
                hasShadowComponent.instance.shadow.enabled = hasShadowComponent.distanceToGround < 1;
            }
            
            foreach (var entity in world.GetFilter<HasShadowComponent>()
                         .Inc<ModelComponent>()
                         .Inc<LookingDirection>()
                         .Inc<PositionComponent>()
                         .Exc<DisabledComponent>()
                         .End())
            {
                var hasShadowComponent = hasShadowComponents.Get(entity);
                var modelComponent = modelComponents.Get(entity);
                var lookingDirection = lookingDirectionComponents.Get(entity);
                var positionComponent = positionComponents.Get(entity);

                if (hasShadowComponent.instance == null)
                {
                    continue;
                }

                if (modelComponent.rotation == ModelComponent.RotationType.Rotate)
                {
                    hasShadowComponent.instance.transform.localScale = Vector3.one;
                    
                    var direction3d = lookingDirection.value;
                    var direction2d = GamePerspective.ProjectFromWorld(new Vector3(direction3d.x, direction3d.y * 0.1f, direction3d.z));

                    var p0 = GamePerspective.ProjectFromWorld(new Vector3(positionComponent.value.x, 0, positionComponent.value.z));
                    var p1 = p0 + direction2d;

                    var angle = Vector2.SignedAngle(Vector2.right, p1 - p0);

                    var t = hasShadowComponent.instance.transform;

                    t.localEulerAngles = new Vector3(0, 0, angle);
                    var shadowScale = t.localScale;
                    t.localScale = new Vector3(Mathf.Clamp(direction2d.magnitude, 0.1f, 1.0f), shadowScale.y, shadowScale.z);
                }
                else if (modelComponent.rotation == ModelComponent.RotationType.FlipToLookingDirection)
                {
                    hasShadowComponent.instance.transform.localScale = new Vector3(1,
                        hasShadowComponent.shadowPerspective, 1);
                }
                    
                var shadowColor = hasShadowComponent.instance.shadow.color;
                var opacityByDistance = 1.0f - hasShadowComponent.distanceToGround;
                shadowColor.a = baseShadowOpacity * modelComponent.color.a * opacityByDistance;
                hasShadowComponent.instance.shadow.color = shadowColor;
            }
            
            foreach (var entity in world.GetFilter<HasShadowComponent>()
                         .Inc<ModelComponent>()
                         .Exc<DisabledComponent>()
                         .End())
            {
                var hasShadowComponent = hasShadowComponents.Get(entity);
                var modelComponent = modelComponents.Get(entity);

                if (hasShadowComponent.shadowEntity.Exists())
                {
                    if (hasShadowComponent.shadowEntity.Has<ModelComponent>())
                    {
                        ref var shadowModel = ref hasShadowComponent.shadowEntity.Get<ModelComponent>();
                        shadowModel.visiblity = modelComponent.visiblity;
                    }
                }

                if (hasShadowComponent.instance != null)
                {
                    if (!hasShadowComponent.instance.gameObject.activeSelf)
                    {
                        hasShadowComponent.instance.gameObject.SetActive(true);
                    }
                }
            }
            
            foreach (var entity in world.GetFilter<HasShadowComponent>()
                         .Inc<ModelComponent>()
                         .Exc<DisabledComponent>()
                         // .Exc<SpineComponent>()
                         .End())
            {
                var hasShadowComponent = hasShadowComponents.Get(entity);
                var modelComponent = modelComponents.Get(entity);

                // copy sprite to shadow
                if (hasShadowComponent.instance != null && hasShadowComponent.copyFromModel)
                {
                    hasShadowComponent.instance.shadow.sprite = modelComponent.instance.spriteRenderer.sprite;
                }
            }
            
            foreach (var entity in world.GetFilter<ShadowComponent>()
                         .Inc<ModelComponent>()
                         .Exc<DisabledComponent>()
                         .End())
            {
                var shadowComponent = shadowComponents.Get(entity);
                var modelComponent = modelComponents.Get(entity);

                var shadowColor = modelComponent.instance.spriteRenderer.color;
                var opacityByDistance = 1.0f - shadowComponent.distanceToGround;
                shadowColor.a = baseShadowOpacity * modelComponent.color.a * opacityByDistance;
                modelComponent.instance.spriteRenderer.color = shadowColor;
            }
            
            foreach (var entity in world.GetFilter<HasShadowComponent>()
                         .Inc<DisabledComponent>()
                         .End())
            {
                var hasShadowComponent = hasShadowComponents.Get(entity);

                if (hasShadowComponent.instance != null)
                {
                    if (hasShadowComponent.instance.gameObject.activeSelf)
                    {
                        hasShadowComponent.instance.gameObject.SetActive(false);
                    }
                }

                if (hasShadowComponent.shadowEntity.Exists())
                {
                    if (hasShadowComponent.shadowEntity.Has<ModelComponent>())
                    {
                        ref var shadowModel = ref hasShadowComponent.shadowEntity.Get<ModelComponent>();
                        shadowModel.visiblity = ModelComponent.Visiblity.Hidden;
                    }
                }
            }
        }
    }
}