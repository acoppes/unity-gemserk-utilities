using Gemserk.Leopotam.Ecs;
using UnityEngine;

public class PositionDefinitionObject : ComponentDefinitionBase
{
    public void Apply(World world, Entity entity)
    {
        world.AddComponent(entity, new PositionComponent());
    }
}