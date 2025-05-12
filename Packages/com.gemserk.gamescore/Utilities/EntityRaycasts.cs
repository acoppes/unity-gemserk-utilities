using System;
using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using UnityEngine;
using Vertx.Debugging;

namespace Game.Utilities
{
    public struct EntityHit
    {
        public Entity entity;
        public RaycastHit2D hit;
    }
    
    public static class EntityRaycasts
    {
        // TODO: make optional provide how many hits it wants...
        
        private static RaycastHit2D[] _tempHits = new RaycastHit2D[20];
        
        public static List<EntityHit> GetEntitiesFromPhysics2dRaycast(Vector3 position, Vector3 direction, 
            ContactFilter2D contactFilter2D, float distance, List<EntityHit> entities)
        {
            return GetEntitiesFromPhysics2dRaycast(position, direction, contactFilter2D, distance, entities, null);
        }
        
        public static List<EntityHit> GetEntitiesFromPhysics2dRaycast(Vector3 position, Vector3 direction, 
            ContactFilter2D contactFilter2D, float distance, List<EntityHit> entities, Func<Entity, bool> filter)
        {
            var hitCount = DrawPhysics2D.Raycast(position, direction,
                contactFilter2D, _tempHits, distance);
            
            for (var i = 0; i < hitCount; i++)
            {
                var hit = _tempHits[i];
                
                var entityReference = hit.collider.GetComponentInParent<EntityReference>();

                if (!entityReference || !entityReference.entity)
                {
                    continue;
                }

                if (filter != null && !filter(entityReference.entity))
                {
                    continue;
                }
                
                entities.Add(new EntityHit()
                {
                    entity = entityReference.entity,
                    hit = hit
                });
            }

            return entities;
        }
        
        public static bool GetFirstEntityFromPhysics2dRaycast(Vector3 position, Vector3 direction, 
            ContactFilter2D contactFilter2D, float distance, out Entity entity, Func<Entity, bool> filter)
        {
            entity = Entity.NullEntity;
            
            var raycastHit = DrawPhysics2D.Raycast(position, direction, distance, contactFilter2D.layerMask);

            if (raycastHit.collider)
            {
                var entityReference = raycastHit.collider.GetComponentInParent<EntityReference>();

                if (!entityReference || !entityReference.entity)
                {
                    return false;
                }

                if (!filter(entityReference.entity))
                {
                    return false;
                }

                entity = entityReference.entity;
                return true;
            }
            
            return false;
        }
    }
}