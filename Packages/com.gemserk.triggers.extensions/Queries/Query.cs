using System;
using UnityEngine;

namespace Gemserk.Triggers.Queries
{
    public class Query : MonoBehaviour
    {
        [NonSerialized]
        private EntityQuery entityQuery;
        [NonSerialized]
        private bool cached;

        #if UNITY_EDITOR
        public bool disableEditorAutoName;
        #endif
        
        public EntityQuery GetEntityQuery()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return EntityQuery.Create(GetComponents<IQueryParameter>());
            }
#endif
            if (!cached)
            {
                entityQuery = EntityQuery.Create(GetComponents<IQueryParameter>());
            }
            
            return entityQuery;
        }

        public override string ToString()
        {
            return GetEntityQuery().ToString();
        }
    }
}