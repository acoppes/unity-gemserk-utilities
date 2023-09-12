using System;
using MyBox;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public enum ReferenceType
    {
        Default = 0,
        Name = 1,
        Tag = 2,
        SameScene = 3,
        SceneName = 4
    }
    
    [Serializable]
    public class CachedObjectReference<T> where T: MonoBehaviour
    {
        public ReferenceType type;

        [ConditionalField(nameof(type), false, ReferenceType.Name, ReferenceType.SceneName)]
        public string name;
        
        
        [ConditionalField(nameof(type), false, ReferenceType.Tag)]
        [Tag]
        public string tag;

        public T GetReference(GameObject go = null)
        {
            if (type == ReferenceType.Default)
            {
                return CachedObjectBehaviour<T>.Default;
            }
            
            if (type == ReferenceType.Name)
            {
                return CachedObjectBehaviour<T>.GetByName(name);
            }

            if (type == ReferenceType.Tag)
            {
                return CachedObjectBehaviour<T>.GetByTag(tag);
            }

            if (type == ReferenceType.SameScene && go != null)
            {
                return CachedObjectBehaviour<T>.GetByScene(go.scene);
            }
            
            if (type == ReferenceType.SceneName)
            {
                return CachedObjectBehaviour<T>.GetBySceneName(name);
            }

            return null;
        }
    }
}