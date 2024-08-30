using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public struct LevelComponent : IEntityComponent
    {
        public int max;
        public int current;

        public bool IsMaxLevel => current == max - 1;
    }
    
    public class LevelComponentDefinition : ComponentDefinitionBase
    {
        public int maxLevel;
        public int startingLevel;
        
        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new LevelComponent
            {
                max = maxLevel,
                current = startingLevel
            });
        }
    }
}