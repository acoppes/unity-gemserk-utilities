using System;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Scenes.EntityDefinitionField
{
    [MovedFrom(false, "", "Assembly-CSharp", "PositionDefinition")]
    public class PositionDefinition : EntityComponentDefinitionBase
    {
        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new PositionComponent()
            {
                value = gameObject.transform.position
            });   
        }
    }
}

