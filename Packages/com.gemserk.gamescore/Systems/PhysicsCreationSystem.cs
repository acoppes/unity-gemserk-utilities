using Game.Components;
using Game.Utilities;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using Gemserk.Utilities.Pooling;
using Leopotam.EcsLite;
using UnityEngine;

namespace Game.Systems
{
    public class PhysicsCreationSystem : BaseSystem, IEntityCreatedHandler, IEntityDestroyedHandler, IEcsInitSystem
    {
#if UNITY_6000_0_OR_NEWER
        public PhysicsMaterial defaultMaterial;
#else 
        public PhysicMaterial defaultMaterial;
#endif
        
        private GameObjectPoolMap poolMap;
        
        private GameObject circleColliderPrefab, boxColliderPrefab, capsuleColliderPrefab;
        
        public void Init(EcsSystems systems)
        {
            poolMap = new GameObjectPoolMap("~Physics2dObjects");

            circleColliderPrefab = new GameObject("Prefab_CircleCollider");
            circleColliderPrefab.transform.SetParent(transform);
            circleColliderPrefab.AddComponent<SphereCollider>();
            circleColliderPrefab.SetActive(false);
            
            boxColliderPrefab = new GameObject("Prefab_BoxCollider");
            boxColliderPrefab.transform.SetParent(transform);
            boxColliderPrefab.AddComponent<BoxCollider>();
            boxColliderPrefab.SetActive(false);
            
            capsuleColliderPrefab = new GameObject("Prefab_CapsuleCollider");
            capsuleColliderPrefab.transform.SetParent(transform);
            capsuleColliderPrefab.AddComponent<CapsuleCollider>();
            capsuleColliderPrefab.SetActive(false);
        }

        private Collider CreateCollider(int layer, PhysicsComponent physicsComponent)
        {
            if (physicsComponent.shapeType == PhysicsComponent.ShapeType.Circle)
            {
                var colliderObject = poolMap.Get(circleColliderPrefab);
                colliderObject.SetActive(true);
                
                colliderObject.layer = layer;
                
                // colliderObject.transform.parent = physicsComponent.transform;

                var collider = colliderObject.GetComponent<SphereCollider>();
                collider.enabled = true;

                collider.isTrigger = physicsComponent.isTrigger;
                collider.radius = physicsComponent.size.x;
                
                if (physicsComponent.centerType == PhysicsComponent.CenterType.CenterOnBase)
                {
                    collider.center = new Vector3(0, collider.radius, 0);
                }
                else
                {
                    collider.center = physicsComponent.center;
                }
                
                collider.sharedMaterial = defaultMaterial;
                
                // colliderObject.transform.localPosition = new Vector3(0, physicsComponent.size.y, 0);
                // colliderObject.AddComponent<PhysicsCollisionsDelegate>();

                return collider;
            }

            if (physicsComponent.shapeType == PhysicsComponent.ShapeType.Box)
            {
                var colliderObject = poolMap.Get(boxColliderPrefab);
                colliderObject.SetActive(true);
                
                colliderObject.layer = layer;
                // colliderObject.transform.parent = physicsGameObject.transform;
                    
                var collider = colliderObject.GetComponent<BoxCollider>();
                collider.enabled = true;
                
                collider.isTrigger = physicsComponent.isTrigger;
                collider.size = physicsComponent.size;
                
                if (physicsComponent.centerType == PhysicsComponent.CenterType.CenterOnBase)
                {
                    collider.center = new Vector3(0, collider.size.y * 0.5f, 0);
                }
                else
                {
                    collider.center = physicsComponent.center;
                }

                collider.sharedMaterial = defaultMaterial;
                
                // colliderObject.transform.localPosition = new Vector3(0, physicsComponent.size.y / 2, 0);
                // colliderObject.AddComponent<PhysicsCollisionsDelegate>();
                
                return collider;
            }
            
            if (physicsComponent.shapeType == PhysicsComponent.ShapeType.Capsule)
            {
                var colliderObject = poolMap.Get(capsuleColliderPrefab);
                colliderObject.SetActive(true);
                
                colliderObject.layer = layer;
                // colliderObject.transform.parent = physicsGameObject.transform;
                    
                var collider = colliderObject.GetComponent<CapsuleCollider>();
                collider.enabled = true;
                
                collider.isTrigger = physicsComponent.isTrigger;
                collider.radius = physicsComponent.size.x;
                collider.height = physicsComponent.size.y;
                
                if (physicsComponent.centerType == PhysicsComponent.CenterType.CenterOnBase)
                {
                    collider.center = new Vector3(0, physicsComponent.size.y / 2, 0);
                }
                else
                {
                    collider.center = physicsComponent.center;
                }
                
                collider.sharedMaterial = defaultMaterial;

                // colliderObject.transform.localPosition = new Vector3(0, physicsComponent.size.y / 2, 0);
                // colliderObject.AddComponent<PhysicsCollisionsDelegate>();
                
                return collider;
            }

            return null;
        }
       
        public void OnEntityCreated(World world, Entity entity)
        {
            if (world.HasComponent<PhysicsComponent>(entity))
            {
                ref var physicsComponent = ref world.GetComponent<PhysicsComponent>(entity);

                if (physicsComponent.prefab != null)
                {
                    physicsComponent.gameObject = GameObject.Instantiate(physicsComponent.prefab);
                    physicsComponent.transform = physicsComponent.gameObject.transform;
                    physicsComponent.gameObject.SetActive(true);
                    
                    physicsComponent.body = physicsComponent.gameObject.GetComponent<Rigidbody>();
                    
                    var entityReference = physicsComponent.gameObject.AddComponent<EntityReference>();
                    entityReference.entity = entity;
                }
                else
                {
                    physicsComponent.gameObject = new GameObject("~PhysicsObject");
                    physicsComponent.transform = physicsComponent.gameObject.transform;

                    var entityReference = physicsComponent.gameObject.AddComponent<EntityReference>();
                    entityReference.entity = entity;

                    var layer = physicsComponent.isStatic ? LayerMask.NameToLayer("StaticObstacle") : 
                        LayerMask.NameToLayer("DynamicObstacle");
                    
                    if (physicsComponent.isStatic)
                    {
                        physicsComponent.body = null;
                        physicsComponent.gameObject.isStatic = true;
                     
                        // obstacle.body.constraints = RigidbodyConstraints.FreezeAll;
                    }
                    else
                    {
                        physicsComponent.body = physicsComponent.gameObject.AddComponent<Rigidbody>();

                        // physicsComponent.body.drag = 0;
                        physicsComponent.angularDamping = 10;
                        physicsComponent.body.useGravity = false;
                        physicsComponent.body.mass = physicsComponent.mass;

                        physicsComponent.body.constraints = RigidbodyConstraints.FreezeRotation;
                        physicsComponent.body.collisionDetectionMode = CollisionDetectionMode.Continuous;

                        if (world.HasComponent<PositionComponent>(entity))
                        {
                            var position = world.GetComponent<PositionComponent>(entity);
                            physicsComponent.body.position = position.value;
                        }
                    }

                    if (physicsComponent.colliderType.HasColliderType(PhysicsComponent.ColliderType.CollideWithDynamicObstacles))
                    {
                        physicsComponent.collideWithDynamicCollider = CreateCollider(layer, physicsComponent);
                        physicsComponent.collideWithDynamicCollider.transform.SetParent(physicsComponent.gameObject.transform, false);
                        physicsComponent.collideWithDynamicCollider.transform.localPosition = Vector3.zero;
                    }

                    if (!physicsComponent.isStatic)
                    {
                        if (physicsComponent.colliderType.HasColliderType(PhysicsComponent.ColliderType.CollideWithStaticObstacles))
                        {
                            physicsComponent.collideWithStaticCollider =
                                CreateCollider(LayerMask.NameToLayer("CollideWithStaticObstacles"), physicsComponent);
                            physicsComponent.collideWithStaticCollider.transform.SetParent(physicsComponent.gameObject.transform, false);
                            physicsComponent.collideWithStaticCollider.transform.localPosition = Vector3.zero;
                        }
                    }
                }

                if (physicsComponent.body != null)
                {
                    physicsComponent.gameObject.AddComponent<PhysicsCollisionsDelegate>();
                    physicsComponent.collisionsEventsDelegate =
                        physicsComponent.gameObject.AddComponent<EntityCollisionDelegate>();
                    physicsComponent.collisionsEventsDelegate.world = world;
                    physicsComponent.collisionsEventsDelegate.entity = world.GetEntity(entity);
                }
            }
        }
        
        public void OnEntityDestroyed(World world, Entity entity)
        {
            if (world.HasComponent<PhysicsComponent>(entity))
            {
                ref var physicsComponent = ref world.GetComponent<PhysicsComponent>(entity);
                
                if (physicsComponent.collideWithDynamicCollider != null)
                {
                    physicsComponent.collideWithDynamicCollider.transform.SetParent(null);
                    poolMap.Release(physicsComponent.collideWithDynamicCollider.gameObject);
                    physicsComponent.collideWithDynamicCollider = null;
                }
                
                if (physicsComponent.collideWithStaticCollider != null)
                {
                    physicsComponent.collideWithStaticCollider.transform.SetParent(null);
                    poolMap.Release(physicsComponent.collideWithStaticCollider.gameObject);
                    physicsComponent.collideWithStaticCollider = null;
                }
                
                if (physicsComponent.collisionsEventsDelegate != null)
                {
                    physicsComponent.collisionsEventsDelegate.onCollisionEnter = null;
                }

                if (physicsComponent.gameObject != null)
                {
                    GameObject.Destroy(physicsComponent.gameObject);
                }

                physicsComponent.gameObject = null;
                physicsComponent.transform = null;
                physicsComponent.body = null;
            }
        }
    }
}