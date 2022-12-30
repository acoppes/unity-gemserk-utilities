using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public interface IEntityDefinition
    {
        void Apply(World world, Entity entity);
    }
    
    public interface IEntityComponentDefinition
    {
        void Apply(World world, Entity entity);
    }

    public abstract class EntityComponentDefinitionBase : IEntityComponentDefinition
    {
        [HideInInspector]
        public string name;

        public EntityComponentDefinitionBase()
        {
            name = GetType().Name.Replace("Definition", "");
        }
        
        public virtual void Apply(World world, Entity entity)
        {
            
        }
    }
}