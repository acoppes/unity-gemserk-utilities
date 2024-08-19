using Game.Utilities;
using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public struct TargetComponent : IEntityComponent
    {
        public Target target;
    }

    public class TargetComponentDefinition : ComponentDefinitionBase
    {
        public TargetType targetType = TargetType.TargetType0;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new TargetComponent()
            {
                target = new Target
                {
                    entity = entity,
                    targetType = targetType
                }
            });
        }
    }
}