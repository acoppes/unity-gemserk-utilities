using System;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Game.Utilities
{
    public class EntityCollision2dDelegate : MonoBehaviour
    {
        [NonSerialized]
        public World world;
        
        [NonSerialized]
        public Entity entity;
        
        public IEntityCollisionDelegate.CollisionHandler onCollisionEnter;
        
        private void OnCollisionEnter2D(Collision2D collision)
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
                    collider2D = collision.collider,
                    collision2D = collision
                    // collision = collision
                });
            }
        }
        
        private void OnTriggerEnter2D(Collider2D collider)
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
                    collider2D = collider,
                    collision2D = null
                    // collision = collision
                });
            }
        }
    }
}