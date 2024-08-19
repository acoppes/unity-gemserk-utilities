using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public class PositionComponentDefinition : ComponentDefinitionBase
    {
        public int type;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new PositionComponent()
            {
                type = type
            });
        }
    }
}