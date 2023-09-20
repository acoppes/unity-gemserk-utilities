using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Game.Utilities
{
    public interface IEntityCollisionDelegate
    {
        public struct EntityCollision
        {
            public Entity entity;
            public bool isTrigger;
            public Collider collider;
            
            public Collider2D collider2D;
            public Collision2D collision2D;
        }
        
        public delegate void CollisionHandler(World world, Entity entity, EntityCollision entityCollision);
    }
}