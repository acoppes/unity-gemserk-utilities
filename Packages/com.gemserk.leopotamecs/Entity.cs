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
        public static void Add<T>(this Entity entity) where T : struct
        {
            entity.world.AddComponent<T>(entity);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add<T>(this Entity entity, T t) where T : struct
        {
            entity.world.AddComponent(entity, t);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T AddOrSet<T>(this Entity entity, T t) where T : struct
        {
            return ref entity.world.AddOrSetComponent(entity, t);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T Get<T>(this Entity entity) where T : struct
        {
            return ref entity.world.GetComponent<T>(entity);
        }
        
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static bool TryGet<T>(this Entity entity, out T t) where T : struct
        // {
        //     if (entity.Has<T>())
        //     {
        //         t = entity.Get<T>();
        //         return true;
        //     }
        //     t = default;
        //     return false;
        // }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Has<T>(this Entity entity) where T : struct
        {
            return entity.world.HasComponent<T>(entity);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Remove<T>(this Entity entity) where T : struct
        {
            entity.world.RemoveComponent<T>(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Exists(this Entity entity)
        {
            if (entity == Entity.NullEntity)
            {
                return false;
            }
            return entity.world && entity.world.Exists(entity);
        }
    }
    
    // This is a higher concept of Entity, not the ecs entity itself, it has useful methods and data to simplify
    // accessing the ecs layer. 
    public struct Entity : IEquatable<Entity>
    {
        public static readonly Entity NullEntity = new Entity()
        {
            world = null,
            ecsGeneration = 0,
            ecsEntity = 0
        };
        
        public World world;
        
        public int ecsEntity;
        public short ecsGeneration;
        
        public static implicit operator bool(Entity entity)
        {
            return entity.Exists();
        }

        public static Entity[] CreateArray(int count)
        {
            var entities = new Entity[count];
            for (var i = 0; i < count; i++)
            {
                entities[i] = NullEntity;
            }
            return entities;
        }
        
        public static void CreateArray(Entity[] entityArray)
        {
            for (var i = 0; i < entityArray.Length; i++)
            {
                entityArray[i] = NullEntity;
            }
        }

        public static Entity Create(World world, int entity, short generation)
        {
            return new Entity
            {
                world = world,
                ecsEntity = entity,
                ecsGeneration = generation
            };
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ecsEntity, ecsGeneration);
        }

        public static implicit operator int(Entity entity) => entity.ecsEntity;

        public static bool operator ==(Entity a, Entity b)
        {
            return a.ecsEntity == b.ecsEntity && a.ecsGeneration == b.ecsGeneration && a.world == b.world;
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return a.ecsEntity != b.ecsEntity || a.ecsGeneration != b.ecsGeneration || a.world != b.world;
        }

        public override bool Equals(object obj)
        {
            if (obj is int e)
            {
                return ecsEntity == e;
            }

            if (obj is Entity entity)
            {
                return Equals(entity);
            }
            
            return false;
        }
        
        public bool Equals(Entity other)
        {
            return this == other;
        }

        public override string ToString()
        {
            return $"[{ecsEntity},{ecsGeneration}]";
        }
    }
}