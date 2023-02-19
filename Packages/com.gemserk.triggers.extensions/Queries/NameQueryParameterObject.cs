using System;
using Gemserk.Leopotam.Ecs;
using UnityEngine.Assertions;

namespace Gemserk.Triggers.Queries
{
    public class NameQueryParameterObject : QueryParameterBase
    {
        public string name;
        
        public override bool MatchQuery(World world, Entity entity)
        {
            Assert.IsFalse(string.IsNullOrEmpty(name), "Cant filter without name");

            if (!world.HasComponent<NameComponent>(entity))
                return false;

            var nameComponent = world.GetComponent<NameComponent>(entity);

            return name.Equals(nameComponent.name, StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            return $"name:{name}";
        }
    }
}