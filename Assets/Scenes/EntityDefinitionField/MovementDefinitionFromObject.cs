using Gemserk.Leopotam.Ecs;
using UnityEngine;

public class MovementDefinitionFromObject : ComponentDefinitionBase
{
    public int speed;
    
    public void Apply(World world, Entity entity)
    {
        world.AddComponent(entity, new MyMovementComponent()
        {
            speed = speed
        });
    }
}