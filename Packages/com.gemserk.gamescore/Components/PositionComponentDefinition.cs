using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public class PositionComponentDefinition : ComponentDefinitionBase
    {
        public int type;
        
        public override string GetComponentName()
        {
            return nameof(PositionComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new PositionComponent()
            {
                type = type
            });
        }
    }
}