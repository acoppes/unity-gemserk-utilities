using Leopotam.EcsLite;

namespace Gemserk.Leopotam.Ecs
{
    public static class EntityWorldExtensions
    {
        public static Entity CreateEmptyEntity(this EcsWorld world)
        {
            var entity = world.NewEntity();
            return new Entity()
            {
                entity = entity,
                generation = world.GetEntityGen(entity)
            };
        }
        
        public static Entity GetEntity(this BaseSystem baseSystem, int entity)
        {
            return baseSystem.world.GetEntity(entity);
        }
    }
    
    public struct Entity
    {
        public static Entity NullEntity = new Entity
        {
            entity = -1,
            generation = -1
        };
        
        public int entity;
        public short generation;

        public override int GetHashCode()
        {
            return base.GetHashCode();
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