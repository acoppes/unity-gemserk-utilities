using System.Collections.Generic;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public class ObjectEntityDefinition : MonoBehaviour
    {
        [SerializeReference]
        public List<IEntityComponentDefinition> componentDefinitions = new List<IEntityComponentDefinition>();
    }
}