using System;
using Gemserk.Leopotam.Ecs;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace Gemserk.Triggers.Queries
{
    public readonly struct NameParameter : IQueryParameter
    {
        private readonly string name;

        public NameParameter(string name)
        {
            this.name = name;
        }
        
        public bool MatchQuery(Entity entity)
        {
            if (!entity.Has<NameComponent>())
            {
                return false;
            }
            
            return name.Equals(entity.Get<NameComponent>().name, 
                StringComparison.OrdinalIgnoreCase);
        }
    }
    
    public class NameQueryParameter : QueryParameterBase
    {
        [FormerlySerializedAs("name")]
        public string objectName;
        
        public override bool MatchQuery(Entity entity)
        {
            Assert.IsFalse(string.IsNullOrEmpty(objectName), 
                "Cant filter without name");
            return new NameParameter(objectName).MatchQuery(entity);
        }

        public override string ToString()
        {
            return $"name:{objectName}";
        }
    }
}