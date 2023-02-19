using Gemserk.Leopotam.Ecs;
using UnityEngine;

public class MovementDefinitionFromObject : ComponentDefinitionBase
{
    public int speed;

    public override string GetComponentName()
    {
        return nameof(MyMovementComponent);
    }

    public override void Apply(World world, Entity entity)
    {
        world.AddComponent(entity, new MyMovementComponent()
        {
            speed = speed
        });
    }
}