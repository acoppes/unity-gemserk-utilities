using System;

namespace Gemserk.Leopotam.Ecs
{
    public static class EntityWorldExtensions
    {
        public static Entity GetEntity(this BaseSystem baseSystem, int entity)
        {
            return baseSystem.world.GetEntity(entity);
        }
        
        public static ref T Get<T>(this Entity entity) where T : struct
        {
            return ref entity.world.GetComponent<T>(entity);
        }
        
        public static bool Has<T>(this Entity entity) where T : struct
        {
            return entity.world.HasComponent<T>(entity);
        }
        
    }
    
    public struct Entity
    {
        public static readonly Entity NullEntity = new Entity(null, -1, -1);
        
        public World world;
        public int entity;
        public short generation;

        public static Entity Create(World world, int entity, short generation)
        {
            return new Entity(world, entity, generation);
        }

        public Entity(World world, int entity, short generation)
        {
            this.world = world;
            this.entity = entity;
            this.generation = generation;
        }

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