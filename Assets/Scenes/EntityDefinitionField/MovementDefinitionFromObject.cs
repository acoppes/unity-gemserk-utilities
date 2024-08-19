using Gemserk.Leopotam.Ecs;

public class MovementDefinitionFromObject : ComponentDefinitionBase
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