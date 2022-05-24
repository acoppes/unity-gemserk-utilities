namespace Gemserk.Leopotam.Ecs
{
    public struct Entity
    {
        public static Entity NullEntity = new Entity
        {
            entity = -1
        };
        
        public int entity;

        public override int GetHashCode()
        {
            return entity;
        }
   
        public static implicit operator Entity(int entity) => new Entity
        {
            entity = entity
        };
        
        public static implicit operator int(Entity entity) => entity.entity;

        public static bool operator ==(Entity reference, int entity)
        {
            return reference.entity == entity;
        }

        public static bool operator !=(Entity reference, int entity)
        {
            return reference != entity;
        }
        
        public static bool operator ==(Entity reference, Entity e)
        {
            return reference.entity == e.entity;
        }

        public static bool operator !=(Entity reference, Entity e)
        {
            return reference.entity != e.entity;
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
    }
}