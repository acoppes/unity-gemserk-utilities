using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;

namespace Game.Components
{
    public struct TimerComponent : IEntityComponent
    {
        public Cooldown timer;
    }
    
    public class TimerComponentDefinition : ComponentDefinitionBase
    {
        public float startingTime;
        
        public override string GetComponentName()
        {
            return nameof(TimerComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new TimerComponent()
            {
                timer = new Cooldown(startingTime)
            });
        }
    }
}