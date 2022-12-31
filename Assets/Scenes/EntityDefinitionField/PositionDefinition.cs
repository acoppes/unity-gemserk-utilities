using System;
using Gemserk.Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Scenes.EntityDefinitionField
{
    [MovedFrom(false, "", "Assembly-CSharp", "PositionDefinition")]
    [Serializable]
    public class PositionDefinition : EntityComponentDefinitionBase
    {
        public Vector3 value;
    
        public void Apply(World world, Entity entity)
        {
        
        }
    }
}

