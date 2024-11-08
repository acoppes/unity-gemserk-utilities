using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public struct LevelComponent : IEntityComponent
    {
        public int max;
        public int current;

        // this just stores the previous level before the level up.
        public int previous;
        public int next;

        public int levelUpDifference => current - previous;

        public int visibleLevel => current + 1;

        public bool levelUpLastFrame;

        public bool IsMaxLevel => current == max - 1;

        public void QueueLevelUp()
        {
            next = current + 1;
        }
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
                current = startingLevel,
                next = startingLevel,
                previous = 0
            });
        }
    }
}