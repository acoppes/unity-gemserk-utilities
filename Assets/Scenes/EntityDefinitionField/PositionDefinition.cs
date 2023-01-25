using Gemserk.Leopotam.Ecs;
using UnityEngine.Scripting.APIUpdating;

namespace Scenes.EntityDefinitionField
{
    [MovedFrom(false, "", "Assembly-CSharp", "PositionDefinition")]
    public class PositionDefinition : ComponentDefinitionBase
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

