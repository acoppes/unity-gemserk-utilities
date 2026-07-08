using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers.Queries;

namespace Game.Queries
{
    public readonly struct TypesParameter : IEntityMatcher
    {
        private readonly string type;

        public TypesParameter(string type)
        {
            this.type = type;
        }
        
        public bool Match(Entity entity)
        {
            if (!entity.Has<TypesComponent>())
            {
                return false;
            }

            var typesComponent = entity.Get<TypesComponent>();
            return typesComponent.types.Contains(type);
        }
    }
    
    public class TypesQueryParameter : EntityMatcherBase
    {
        public string type;

        public override bool Match(Entity entity)
        {
            return new TypesParameter(type).Match(entity);
        }

        public override string ToString()
        {
            return $"hasTypes({type})";
        }
    }
}