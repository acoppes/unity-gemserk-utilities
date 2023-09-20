using Game.Components;
using Gemserk.BitmaskTypes;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers.Queries;

namespace Game.Queries
{
    public readonly struct TypesParameter : IQueryParameter
    {
        private readonly string type;

        public TypesParameter(string type)
        {
            this.type = type;
        }
        
        public bool MatchQuery(Entity entity)
        {
            if (!entity.Has<TypesComponent>())
            {
                return false;
            }

            var typesComponent = entity.Get<TypesComponent>();
            return typesComponent.types.Contains(type);
        }
    }
    
    public class TypesQueryParameter : QueryParameterBase
    {
        public string type;

        public override bool MatchQuery(Entity entity)
        {
            return new TypesParameter(type).MatchQuery(entity);
        }

        public override string ToString()
        {
            return $"hasTypes({type})";
        }
    }
}