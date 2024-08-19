using Gemserk.Leopotam.Ecs;

public class PositionDefinitionObject : ComponentDefinitionBase
{
    public override void Apply(World world, Entity entity)
    {
        world.AddComponent(entity, new PositionComponent());
    }
}