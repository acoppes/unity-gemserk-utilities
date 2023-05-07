using System;

namespace Gemserk.Leopotam.Ecs
{
    public static class EntityWorldExtensions
    {
        public static Entity GetEntity(this BaseSystem baseSystem, int entity)
        {
            return baseSystem.world.GetEntity(entity);
        }
        
        public static T GetComponent<T>(this Entity entity) where T : struct
        {
            return entity.world.GetComponent<T>(entity);
        }
    }
    
    public struct Entity
    {
        public static Entity NullEntity = new Entity
        {
            entity = -1,
            generation = -1,
            world = null
        };
        
        public int entity;
        public short generation;
        public World world;

        public override int GetHashCode()
        {
            return HashCode.Combine(entity, generation);
        }

        public static implicit operator int(Entity entity) => entity.entity;

        public static bool operator ==(Entity reference, Entity e)
        {
            return reference.entity == e.entity && reference.generation == e.generation;
        }

        public static bool operator !=(Entity reference, Entity e)
        {
            return reference.entity != e.entity || reference.generation != e.generation;
        }

        public override bool Equals(object obj)
        {
            if (obj is int entity)
            {
                return this.entity == entity;
            }
            return base.Equals(obj);
        }
        
        public bool Equals(Entity other)
        {
            return entity == other.entity;
        }

        public override string ToString()
        {
            return $"[{entity},{generation}]";
        }
    }
}