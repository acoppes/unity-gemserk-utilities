using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using MyBox;

namespace Game.Components
{
    public struct TimeToLiveComponent : IEntityComponent
    {
        public Cooldown ttl;
    }
    
    public class TimeToLiveComponentDefinition : ComponentDefinitionBase
    {
        public float timeToLive;
        public RangedFloat random;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new TimeToLiveComponent()
            {
                ttl = new Cooldown(timeToLive + random.RandomInRange()),
            });
        }
    }
}