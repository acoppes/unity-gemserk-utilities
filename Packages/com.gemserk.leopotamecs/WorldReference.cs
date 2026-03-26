using System;
using MyBox;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    [Serializable]
    public class WorldReference
    {
        public ReferenceType type;

        [ConditionalField(nameof(type), false, ReferenceType.Name, ReferenceType.SceneName)]
        public string name;
        
        [ConditionalField(nameof(type), false, ReferenceType.Tag)]
        [Tag]
        public string tag;

        public World GetReference(GameObject go = null)
        {
            if (type == ReferenceType.Default)
            {
                return WorldInstances.Default;
            }
            
            if (type == ReferenceType.Name)
            {
                return WorldInstances.GetByName(name);
            }

            if (type == ReferenceType.Tag)
            {
                return WorldInstances.GetByTag(tag);
            }

            if (type == ReferenceType.SameScene && go)
            {
                return WorldInstances.GetByScene(go.scene);
            }
            
            if (type == ReferenceType.SceneName)
            {
                return WorldInstances.GetBySceneName(name);
            }

            return null;
        }
    }
}