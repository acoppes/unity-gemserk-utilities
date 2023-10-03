using Game.Components;
using Game.Utilities;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities.Pooling;
using Leopotam.EcsLite;
using UnityEngine;

namespace Game.Systems
{
    public class Physics2dCreationSystem : BaseSystem, IEntityCreatedHandler, IEntityDestroyedHandler, IEcsInitSystem
    {
        public PhysicsMaterial2D defaultMaterial2d;

        private GameObjectPoolMap poolMap;

        [SerializeField]
        private GameObject circleColliderPrefab, boxColliderPrefab, capsuleColliderPrefab;

        public void Init(EcsSystems systems)
        {
            poolMap = new GameObjectPoolMap("~Physics2dObjects");
        }

        private Collider2D CreateCollider2d(int layer, Physics2dComponent physics2dComponent)
        {
            if (physics2dComponent.shapeType == PhysicsComponent.ShapeType.Circle)
            {
                var colliderObject = poolMap.Get(circleColliderPrefab);
                
                // var colliderObject = new GameObject(GeneratedColliderName);
                colliderObject.layer = layer;
                    
                var collider = colliderObject.GetComponent<CircleCollider2D>();
                
                collider.isTrigger = physics2dComponent.isTrigger;
                collider.radius = physics2dComponent.size.x;
                collider.enabled = true;
                
                if (physics2dComponent.centerType == PhysicsComponent.CenterType.CenterOnBase)
                {
                    collider.offset = new Vector3(0, physics2dComponent.size.y / 2, 0);
                }
                else
                {
                    collider.offset = physics2dComponent.center;
                }
                
                collider.sharedMaterial = defaultMaterial2d;

                // colliderObject.AddComponent<Physics2dCollisionsDelegate>();
                
                return collider;
            }

            if (physics2dComponent.shapeType == PhysicsComponent.ShapeType.Box)
            {
                var colliderObject = poolMap.Get(boxColliderPrefab);
                
                // var colliderObject = new GameObject(GeneratedColliderName);
                colliderObject.layer = layer;
                    
                var collider = colliderObject.GetComponent<BoxCollider2D>();
                collider.isTrigger = physics2dComponent.isTrigger;
                collider.size = physics2dComponent.size;
                collider.enabled = true;
                
                if (physics2dComponent.centerType == PhysicsComponent.CenterType.CenterOnBase)
                {
                    collider.offset = new Vector3(0, physics2dComponent.size.y / 2, 0);
                }
                else
                {
                    collider.offset = physics2dComponent.center;
                }
                
                collider.sharedMaterial = defaultMaterial2d;

                // colliderObject.AddComponent<Physics2dCollisionsDelegate>();
                
                return collider;
            }
            
            if (physics2dComponent.shapeType == PhysicsComponent.ShapeType.Capsule)
            {
                var colliderObject = poolMap.Get(capsuleColliderPrefab);
                // var colliderObject = new GameObject(GeneratedColliderName);
                colliderObject.layer = layer;
                // colliderObject.transform.parent = physicsGameObject.transform;
                    
                var collider = colliderObject.GetComponent<CapsuleCollider2D>();
                
                collider.isTrigger = physics2dComponent.isTrigger;
                collider.size = physics2dComponent.size;
                collider.enabled = true;
                
                if (physics2dComponent.centerType == PhysicsComponent.CenterType.CenterOnBase)
                {
                    collider.offset = new Vector3(0, physics2dComponent.size.y / 2, 0);
                }
                else
                {
                    collider.offset = physics2dComponent.center;
                }
                
                collider.sharedMaterial = defaultMaterial2d;

                // colliderObject.transform.localPosition = new Vector3(0, physicsComponent.size.y / 2, 0);
                // colliderObject.GetOrAddComponent<Physics2dCollisionsDelegate>();
                
                return collider;
            }

            return null;
        }
        
        public void OnEntityCreated(World world, Entity entity)
        {
            if (world.HasComponent<Physics2dComponent>(entity))
            {
                ref var physics2dComponent = ref world.GetComponent<Physics2dComponent>(entity);

                if (physics2dComponent.prefab != null)
                {
                    physics2dComponent.gameObject = GameObject.Instantiate(physics2dComponent.prefab);
                    physics2dComponent.gameObject.SetActive(true);
                    
                    physics2dComponent.body = physics2dComponent.gameObject.GetComponent<Rigidbody2D>();
                    
                    var entityReference = physics2dComponent.gameObject.AddComponent<EntityReference>();
                    entityReference.entity = entity;

                    physics2dComponent.gameObject.GetComponentsInChildren(physics2dComponent.colliders);
                    
                    var position = world.GetComponent<PositionComponent>(entity);
                    physics2dComponent.body.position = position.value;
                }
                else
                {
                    physics2dComponent.gameObject = new GameObject("~Physics2dObject");
   
                    var entityReference = physics2dComponent.gameObject.AddComponent<EntityReference>();
                    entityReference.entity = entity;

                    var layer = physics2dComponent.isStatic ? LayerMask.NameToLayer("StaticObstacle") : 
                        LayerMask.NameToLayer("DynamicObstacle");
                    
                    if (physics2dComponent.isStatic)
                    {
                        physics2dComponent.body = null;
                        physics2dComponent.gameObject.isStatic = true;
                        // obstacle.body.constraints = RigidbodyConstraints.FreezeAll;
                        
                        if (world.HasComponent<PositionComponent>(entity))
                        {
                            var position = world.GetComponent<PositionComponent>(entity);
                            physics2dComponent.gameObject.transform.position = position.value;
                        }
                    }
                    else
                    {
                        physics2dComponent.body = physics2dComponent.gameObject.AddComponent<Rigidbody2D>();

                        // physicsComponent.body.drag = 0;
                        physics2dComponent.body.angularDrag = 10;
                        physics2dComponent.body.gravityScale = 0;
                        physics2dComponent.body.mass = physics2dComponent.mass;

                        physics2dComponent.body.freezeRotation = true;
                        physics2dComponent.body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

                        physics2dComponent.body.bodyType = physics2dComponent.startingBodyType;
                        
                        if (world.HasComponent<PositionComponent>(entity))
                        {
                            var position = world.GetComponent<PositionComponent>(entity);
                            physics2dComponent.body.position = position.value;
                        }
                    }

                    if (physics2dComponent.colliderType.HasColliderType(PhysicsComponent.ColliderType.CollideWithDynamicObstacles))
                    {
                        physics2dComponent.collideWithDynamicCollider = CreateCollider2d(layer, physics2dComponent);
                        physics2dComponent.collideWithDynamicCollider.transform.SetParent(physics2dComponent.gameObject.transform, false);
                        physics2dComponent.collideWithDynamicCollider.transform.localPosition = Vector3.zero;
                    }

                    if (!physics2dComponent.isStatic)
                    {
                        if (physics2dComponent.colliderType.HasColliderType(PhysicsComponent.ColliderType.CollideWithStaticObstacles))
                        {
                            physics2dComponent.collideWithStaticCollider =
                                CreateCollider2d(LayerMask.NameToLayer("CollideWithStaticObstacles"), physics2dComponent);
                            physics2dComponent.collideWithStaticCollider.transform.SetParent(physics2dComponent.gameObject.transform, false);
                            physics2dComponent.collideWithStaticCollider.transform.localPosition = Vector3.zero;
                        }
                    }
                }

                if (physics2dComponent.body != null)
                {
                    physics2dComponent.collisionsEventsDelegate =
                        physics2dComponent.gameObject.AddComponent<EntityCollision2dDelegate>();
                    physics2dComponent.collisionsEventsDelegate.world = world;
                    physics2dComponent.collisionsEventsDelegate.entity = world.GetEntity(entity);
                }
            }
        }
        
        public void OnEntityDestroyed(World world, Entity entity)
        {
            if (entity.Has<Physics2dComponent>())
            {
                ref var physics2dComponent = ref entity.Get<Physics2dComponent>();
                
                physics2dComponent.colliders.Clear();

                if (physics2dComponent.collideWithDynamicCollider != null)
                {
                    physics2dComponent.collideWithDynamicCollider.transform.SetParent(null);
                    poolMap.Release(physics2dComponent.collideWithDynamicCollider.gameObject);
                    physics2dComponent.collideWithDynamicCollider = null;
                }
                
                if (physics2dComponent.collideWithStaticCollider != null)
                {
                    physics2dComponent.collideWithStaticCollider.transform.SetParent(null);
                    poolMap.Release(physics2dComponent.collideWithStaticCollider.gameObject);
                    physics2dComponent.collideWithStaticCollider = null;
                }

                if (physics2dComponent.collisionsEventsDelegate != null)
                {
                    physics2dComponent.collisionsEventsDelegate.onCollisionEnter = null;
                }
                
                physics2dComponent.collisionsEventsDelegate = null;
                physics2dComponent.contacts.Clear();
                
                if (physics2dComponent.gameObject != null)
                {
                    GameObject.Destroy(physics2dComponent.gameObject);
                }

                physics2dComponent.gameObject = null;
                physics2dComponent.body = null;
            }
        }


    }
}