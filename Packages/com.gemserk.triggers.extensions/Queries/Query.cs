using Gemserk.Utilities;

namespace Gemserk.Triggers.Queries
{
    public class Query : AutoNamedObject
    {
        public EntityQuery GetEntityQuery() => EntityQuery.Create(GetComponents<IQueryParameter>());

        public override string GetObjectName()
        {
            return $"Q({GetEntityQuery()})";
        }

        public override string ToString()
        {
            return GetEntityQuery().ToString();
        }
    }
}