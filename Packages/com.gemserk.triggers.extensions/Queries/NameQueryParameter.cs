using System;
using Gemserk.Leopotam.Ecs;
using UnityEngine.Assertions;

namespace Gemserk.Triggers.Queries
{
    public struct NameParameter : IQueryParameter
    {
        public string name;

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
        public string name;
        
        public override bool MatchQuery(Entity entity)
        {
            Assert.IsFalse(string.IsNullOrEmpty(name), 
                "Cant filter without name");
            return new NameParameter(name).MatchQuery(entity);
        }

        public override string ToString()
        {
            return $"name:{name}";
        }
    }
}