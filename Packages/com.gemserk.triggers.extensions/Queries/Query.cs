using UnityEngine;

namespace Gemserk.Triggers.Queries
{
    public class Query : MonoBehaviour
    {
        private EntityQuery entityQuery;
        
        public EntityQuery GetEntityQuery()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return EntityQuery.Create(GetComponents<IQueryParameter>());
            }
#endif
            return entityQuery;
        }

        private void Awake()
        {
            entityQuery = EntityQuery.Create(GetComponents<IQueryParameter>());
        }

        public override string ToString()
        {
            return GetEntityQuery().ToString();
        }
    }
}