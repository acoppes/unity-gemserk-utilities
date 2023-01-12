using System.Collections.Generic;

namespace Gemserk.Leopotam.Ecs
{
    public struct EntityDefinitionComponent
    {
        public IEntityDefinition definition;
        public IEnumerable<IEntityInstanceParameter> parameters;
    }
}