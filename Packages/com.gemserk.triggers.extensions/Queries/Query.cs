using System;
using UnityEngine;

namespace Gemserk.Triggers.Queries
{
    public class Query : MonoBehaviour, ITriggerDebugNamedObject
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
                return EntityQuery.Create(GetComponents<IEntityMatcher>());
            }
#endif
            if (!cached)
            {
                entityQuery = EntityQuery.Create(GetComponents<IEntityMatcher>());
            }
            
            return entityQuery;
        }

        public override string ToString()
        {
            return GetEntityQuery().ToString();
        }

        public string GetObjectName()
        {
#if UNITY_EDITOR
            if (!disableEditorAutoName)
            {
                return $"Q({GetEntityQuery()})";
            }
#endif
            return name;
        }
    }
}