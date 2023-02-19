using Gemserk.Leopotam.Ecs;
using UnityEngine;

public class PositionDefinitionObject : ComponentDefinitionBase
{
    public override string GetComponentName()
    {
        return nameof(PositionComponent);
    }
    
    public override void Apply(World world, Entity entity)
    {
        world.AddComponent(entity, new PositionComponent());
    }
}