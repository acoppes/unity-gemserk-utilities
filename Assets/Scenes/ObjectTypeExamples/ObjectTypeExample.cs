using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using UnityEngine;

namespace Scenes.ObjectTypeExamples
{
    public class ObjectTypeExample : MonoBehaviour
    {
        [ObjectType(typeof(IEntityDefinition), prefabReferencesOnWhenStart = true)]
        public Object referenceToSomethingWithWhatIWant;
        
        [ObjectType(typeof(IEntityDefinition), prefabReferencesOnWhenStart = true)]
        public List<Object> referencesList;
    
        void Start()
        {
            var definition = referenceToSomethingWithWhatIWant.GetInterface<IEntityDefinition>();   
        }
    }
}
