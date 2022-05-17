using System.Collections.Generic;

namespace Gemserk.Leopotam.Ecs.Extensions
{
    public struct CreateEntity
    {
        public IEntityDefinition definition;
        public List<IEntityInstanceParameter> parameters;
    }
}