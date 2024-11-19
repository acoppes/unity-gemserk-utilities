using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public struct XpComponent : IEntityComponent
    {
        public float pending;
        
        public float current;
        
        public float baseTotal;

        public float total;
        public float totalIncrementPerLevel;
    }
    
    public class XpComponentDefinition : ComponentDefinitionBase
    {
        public float total;
        public float totalIncrementPerLevel;

        public override void Apply(World world, Entity entity)
        {
            
            entity.Add(new XpComponent()
            {
                current = 0,
                baseTotal = total,
                totalIncrementPerLevel = totalIncrementPerLevel
            });
        }
    }
}