using System;
using Gemserk.Leopotam.Ecs;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace Gemserk.Triggers.Queries
{
    public readonly struct NameMatcher : IEntityMatcher
    {
        private readonly string name;

        public NameMatcher(string name)
        {
            this.name = name;
        }
        
        public bool Match(Entity entity)
        {
            if (!entity.Has<NameComponent>())
            {
                return false;
            }
            
            return name.Equals(entity.Get<NameComponent>().name, 
                StringComparison.OrdinalIgnoreCase);
        }
    }
    
    public class NameQueryParameter : EntityMatcherBase
    {
        [FormerlySerializedAs("name")]
        public string objectName;
        
        public override bool Match(Entity entity)
        {
            Assert.IsFalse(string.IsNullOrEmpty(objectName), 
                "Cant filter without name");
            return new NameMatcher(objectName).Match(entity);
        }

        public override string ToString()
        {
            return $"name:{objectName}";
        }
    }
}