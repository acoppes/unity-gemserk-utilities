using Gemserk.Utilities;

namespace Gemserk.Triggers.Queries
{
    public class QueryAttribute : ObjectTypeAttribute
    {
        public QueryAttribute() : base(typeof(Query))
        {
            filterString = "Q";
            disableAssetReferences = true;
            sceneReferencesOnWhenStart = true;
            prefabReferencesOnWhenStart = true;
        }
    }
}