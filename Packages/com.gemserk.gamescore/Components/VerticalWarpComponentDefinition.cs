using Gemserk.Leopotam.Ecs;
using MyBox;

namespace Game.Components
{
    public struct VerticalWarpComponent : IEntityComponent
    {
        public RangedFloat verticalRange;
        public float offset;
    }
    
    public class VerticalWarpComponentDefinition : ComponentDefinitionBase
    {
        public RangedFloat verticalRange;
        public float warpOffset;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new VerticalWarpComponent
            {
                verticalRange = verticalRange,
                offset = warpOffset
            });
        }
    }
}