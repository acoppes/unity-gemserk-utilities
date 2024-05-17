using System;
using System.Collections.Generic;
using Game.Utilities;
using Gemserk.Leopotam.Ecs;
using MyBox;
using UnityEngine;

namespace Game.Components
{
    public struct PhysicsComponent : IEntityComponent
    {
        public static readonly int MaxContacts = 20;
        
        public enum ShapeType
        {
            None = 0,
            Circle = 1,
            Box = 2,
            Capsule = 3
        }
        
        public enum SyncType
        {
            Both = 0,
            FromPhysics = 1,
            // ToPhyiscs = 2
        }
        
        [Flags]
        public enum ColliderType
        {
            None = 0,
            CollideWithDynamicObstacles = 1 << 0,
            CollideWithStaticObstacles = 1 << 1
        }

        public enum CenterType
        {
            CenterOnBase = 0,
            Custom = 1
        }

        public GameObject prefab;
        
        public ShapeType shapeType;

        public SyncType syncType;
        
        public bool disableCollideWithObstacles;
        
        public Vector3 size;

        public CenterType centerType;
        public Vector3 center;
        
        public float mass;
        
        // public int priority;

        public GameObject gameObject;
        public Rigidbody body;
        
        public Collider collideWithDynamicCollider;
        public Collider collideWithStaticCollider;

        public bool isStatic;
        public bool isTrigger;

        public Vector3 velocity;

        public EntityCollisionDelegate collisionsEventsDelegate;

        public ColliderType colliderType;

        public List<Collision> collisions;

        public int contactsCount;
        public ContactPoint[] contacts;
    }
    
    public struct Physics2dComponent : IEntityComponent
    {
        public GameObject prefab;
        
        public PhysicsComponent.ShapeType shapeType;
        public PhysicsComponent.SyncType syncType;
        
        public GameObject gameObject;
        public Rigidbody2D body;
        
        public Collider2D collideWithStaticCollider;
        public Collider2D collideWithDynamicCollider;
        
        public List<Collider2D> colliders;

        public EntityCollision2dDelegate collisionsEventsDelegate;

        public bool isStatic;
        public RigidbodyType2D startingBodyType;
        public bool isTrigger;
        public float mass;
        
        public Vector3 size;
        public PhysicsComponent.CenterType centerType;
        public Vector3 center;
        
        public PhysicsComponent.ColliderType colliderType;
        
        // public List<Collision2D> collisions;
        public List<ContactPoint2D> contacts;

        public bool disableCollisions;

        public bool disableContactsCalculations;
    }
    
    public class PhysicsComponentDefinition : ComponentDefinitionBase
    {
        public enum PhysicsComponentType
        {
            Physics3d = 0,
            Physics2d = 1
        }

        public GameObject prefab;

        public PhysicsComponentType physicsComponentType = PhysicsComponentType.Physics3d;
        
        [ConditionalField(nameof(prefab), true)]
        public PhysicsComponent.ShapeType shapeType = PhysicsComponent.ShapeType.None;

        [ConditionalField(nameof(prefab), true)]
        public Vector3 size = new Vector3(0, 0, 0);

        [ConditionalField(nameof(prefab), true)]
        public PhysicsComponent.CenterType centerType = PhysicsComponent.CenterType.CenterOnBase;
       
        [ConditionalField(nameof(centerType), false, PhysicsComponent.CenterType.Custom)]
        public Vector3 center = new Vector3();

        [ConditionalField(nameof(prefab), true)]
        public float mass = 0.25f;
        
        [ConditionalField(nameof(prefab), true)]
        public bool isStatic;
        
        [ConditionalField(nameof(physicsComponentType), false, PhysicsComponentType.Physics2d)]
        public RigidbodyType2D startingBodyType;
        
        [ConditionalField(nameof(prefab), true)]
        public bool isTrigger;
        
        public PhysicsComponent.SyncType defaultSyncType = PhysicsComponent.SyncType.Both;

        [ConditionalField(nameof(prefab), true)]
        public PhysicsComponent.ColliderType colliderType = PhysicsComponent.ColliderType.CollideWithDynamicObstacles |
                                                            PhysicsComponent.ColliderType.CollideWithStaticObstacles;

        // public RigidbodyConstraints rigidbodyConstraints;

        public bool disableContactsCalculations;
        
        public override string GetComponentName()
        {
            return nameof(PhysicsComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            if (physicsComponentType == PhysicsComponentType.Physics3d)
            {
                if (prefab != null)
                {
                    world.AddComponent(entity, new PhysicsComponent()
                    {
                        prefab = prefab,
                        syncType = defaultSyncType,
                        collisions = new List<Collision>(),
                        contacts = new ContactPoint[PhysicsComponent.MaxContacts]
                    });
                }
                else
                {
                    world.AddComponent(entity, new PhysicsComponent()
                    {
                        size = size,
                        centerType = centerType,
                        center = center,
                        mass = mass,
                        shapeType = shapeType,
                        isStatic = isStatic,
                        isTrigger = isTrigger,
                        syncType = defaultSyncType,
                        colliderType = colliderType,
                        collisions = new List<Collision>(),
                        contacts = new ContactPoint[PhysicsComponent.MaxContacts]
                    });
                }
                

            }
            else
            {
                if (prefab != null)
                {
                    world.AddComponent(entity, new Physics2dComponent()
                    {
                        prefab = prefab,
                        syncType = defaultSyncType,
                        contacts = new List<ContactPoint2D>(),
                        colliders = new List<Collider2D>(), 
                        disableContactsCalculations = disableContactsCalculations
                    });
                }
                else
                {
                    world.AddComponent(entity, new Physics2dComponent()
                    {
                        size = size,
                        centerType = centerType,
                        isStatic = isStatic,
                        isTrigger = isTrigger,
                        mass = mass,
                        shapeType = shapeType,
                        syncType = defaultSyncType,
                        colliderType = colliderType,
                        center = center,
                        contacts = new List<ContactPoint2D>(),
                        colliders = new List<Collider2D>(),
                        startingBodyType = startingBodyType,
                        disableContactsCalculations = disableContactsCalculations
                    });
                }
            }
        }
    }
}