using UnityEngine;

namespace Gemserk.Triggers.Queries
{
    public class Query : MonoBehaviour
    {
        public EntityQuery GetEntityQuery() => EntityQuery.Create(GetComponents<IQueryParameter>());

        public override string ToString()
        {
            return GetEntityQuery().ToString();
        }
    }
}