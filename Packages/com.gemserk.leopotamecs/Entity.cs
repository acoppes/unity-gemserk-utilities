using System;
using System.Runtime.CompilerServices;

namespace Gemserk.Leopotam.Ecs
{
    public static class EntityWorldExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Entity GetEntity(this BaseSystem baseSystem, int entity)
        {
            return baseSystem.world.GetEntity(entity);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T Get<T>(this Entity entity) where T : struct
        {
            return ref entity.world.GetComponent<T>(entity);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Has<T>(this Entity entity) where T : struct
        {
            return entity.world.HasComponent<T>(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Exists(this Entity entity)
        {
            if (entity == Entity.NullEntity)
            {
                return false;
            }
        
            if (entity.world == null)
            {
                return false;
            }
                        
            return entity.world.Exists(entity);
        }
    }
    
    // This is a higher concept of Entity, not the ecs entity itself, it has useful methods and data to simplify
    // accessing the ecs layer. 
    public struct Entity
    {
        public static readonly Entity NullEntity = new Entity(null, -1, -1);
        
        public World world;
        
        public int ecsEntity;
        public short ecsGeneration;

        public static Entity Create(World world, int entity, short generation)
        {
            return new Entity(world, entity, generation);
        }

        public Entity(World world, int ecsEntity, short ecsGeneration)
        {
            this.world = world;
            this.ecsEntity = ecsEntity;
            this.ecsGeneration = ecsGeneration;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ecsEntity, ecsGeneration);
        }

        public static implicit operator int(Entity entity) => entity.ecsEntity;

        public static bool operator ==(Entity reference, Entity e)
        {
            return reference.ecsEntity == e.ecsEntity && reference.ecsGeneration == e.ecsGeneration;
        }

        public static bool operator !=(Entity reference, Entity e)
        {
            return reference.ecsEntity != e.ecsEntity || reference.ecsGeneration != e.ecsGeneration;
        }

        public override bool Equals(object obj)
        {
            if (obj is int entity)
            {
                return this.ecsEntity == entity;
            }
            return base.Equals(obj);
        }
        
        public bool Equals(Entity other)
        {
            return ecsEntity == other.ecsEntity;
        }

        public override string ToString()
        {
            return $"[{ecsEntity},{ecsGeneration}]";
        }
    }
}