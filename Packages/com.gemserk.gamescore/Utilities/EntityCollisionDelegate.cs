using System;
using Game.Components;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Game.Utilities
{
    public class EntityCollisionDelegate : MonoBehaviour
    {
        [NonSerialized]
        public World world;
        
        [NonSerialized]
        public Entity entity;
        
        public IEntityCollisionDelegate.CollisionHandler onCollisionEnter;

        private void OnCollisionEnter(Collision collision)
        {
            if (onCollisionEnter != null)
            {
                var entityReference = collision.gameObject.GetComponentInParent<EntityReference>();
                var collisionEntity = Entity.NullEntity;

                if (entityReference != null)
                {
                    collisionEntity = entityReference.entity;
                }
                
                onCollisionEnter(world, entity, new IEntityCollisionDelegate.EntityCollision
                {
                    entity = collisionEntity,
                    // collision = collision
                });
            }
        }
        
        private void OnCollisionStay(Collision collision)
        {
            ref var physicsComponent = ref world.GetComponent<PhysicsComponent>(entity);
            physicsComponent.collisions.Add(collision);

            for (var i = 0; i < collision.contactCount; i++)
            {
                if (physicsComponent.contactsCount >= PhysicsComponent.MaxContacts)
                    return;
                physicsComponent.contacts[physicsComponent.contactsCount++] = collision.contacts[i];
            }
        }
        
        private void OnTriggerEnter(Collider collider)
        {
            if (onCollisionEnter != null)
            {
                var entityReference = collider.gameObject.GetComponentInParent<EntityReference>();
                var collisionEntity = Entity.NullEntity;

                if (entityReference != null)
                {
                    collisionEntity = entityReference.entity;
                }
                
                onCollisionEnter(world, entity, new IEntityCollisionDelegate.EntityCollision
                {
                    entity = collisionEntity,
                    isTrigger = true,
                    collider = collider
                    // collision = collision
                });
            }
        }
    }
}