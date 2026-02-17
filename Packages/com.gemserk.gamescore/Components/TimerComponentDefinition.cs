using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;

namespace Game.Components
{
    public struct TimerComponent : IEntityComponent
    {
        public Cooldown timer;
        public bool wasReady;
        public bool paused;
    }
    
    public class TimerComponentDefinition : ComponentDefinitionBase
    {
        public float startingTime;
        public bool startsPaused;
        
        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new TimerComponent()
            {
                timer = new Cooldown(startingTime),
                paused = startsPaused
            });
        }
    }
}