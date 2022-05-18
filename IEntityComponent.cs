namespace Gemserk.Leopotam.Ecs
{
    public interface IEntityComponent
    {
        
    }

    public struct EntityReference
    {
        public int entity;

        public bool IsNull => entity < 0;

        public void SetNull() => entity = -1;
        
        public static implicit operator EntityReference(int entity) => new EntityReference
        {
            entity = entity
        };

        public static bool operator ==(EntityReference reference, int entity)
        {
            return reference.entity == entity;
        }

        public static bool operator !=(EntityReference reference, int entity)
        {
            return reference != entity;
        }

        public override bool Equals(object obj)
        {
            if (obj is int entity)
            {
                return this.entity == entity;
            }
            return base.Equals(obj);
        }

        public override string ToString()
        {
            return IsNull ? "NULL" : entity.ToString();
        }
    }
}