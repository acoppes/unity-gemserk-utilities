using Gemserk.Leopotam.Ecs;

public struct MyMovementComponent : IEntityComponent
{
    public int speed;
}

public class MovementDefinition : ComponentDefinitionBase
{
    public int speed;
    
    public override void Apply(World world, Entity entity)
    {
        world.AddComponent(entity, new MyMovementComponent()
        {
            speed = speed
        });
    }
}