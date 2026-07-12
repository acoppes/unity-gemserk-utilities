using Gemserk.Leopotam.Ecs;

namespace Gemserk.Triggers.Queries
{
    public class NotQueryParameter : EntityMatcherBase
    {
        public EntityMatcherBase queryParameter;
        
        public override bool Match(Entity entity)
        {
            return !queryParameter.Match(entity);
        }
    }
}