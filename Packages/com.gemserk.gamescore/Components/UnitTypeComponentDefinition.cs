using Game.DataAssets;
using Gemserk.BitmaskTypes;
using Gemserk.Leopotam.Ecs;
using MyBox;

namespace Game.Components
{
    public struct UnitTypeComponent : IEntityComponent
    {
        public int unitType;
    }
    
    public class UnitTypeComponentDefinition : ComponentDefinitionBase
    {
        public UnitTypeAsset unitTypeAsset;
        
        [ConditionalField(nameof(unitTypeAsset), true)]
        [BitMask(16)]
        public int type;
        
        public override string GetComponentName()
        {
            return nameof(UnitTypeComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            var type = this.type;

            if (unitTypeAsset != null)
            {
                type = unitTypeAsset.type;
            }
            
            world.AddComponent(entity, new UnitTypeComponent()
            {
                unitType = type
            });
        }
    }
}