using Game.Utilities;
using Gemserk.BitmaskTypes;
using Gemserk.Leopotam.Ecs;
using MyBox;

namespace Game.Components
{
    public struct TargetComponent : IEntityComponent
    {
        public Target target;
    }

    public class TargetComponentDefinition : ComponentDefinitionBase
    {
        [ConditionalField(nameof(targetTypeAsset), true)]  
        public TargetType targetType = TargetType.TargetType0;

        public IntTypeAsset targetTypeAsset;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new TargetComponent()
            {
                target = new Target
                {
                    entity = entity,
                    targetType = targetTypeAsset ? targetTypeAsset.value : (int) targetType
                }
            });
        }
    }
}