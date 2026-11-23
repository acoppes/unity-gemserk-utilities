using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using UnityEngine.Serialization;

namespace Game.Components
{
    public struct TimerComponent : IEntityComponent
    {
        public Cooldown timer;
        public bool wasReady;
        public bool paused;
    }
    
    public class TimerComponentDefinition : ComponentDefinitionBase, IEntityInstanceParameter
    {
        [FormerlySerializedAs("startingTime")] 
        public float totalTime;
        
        public bool startsPaused;
        
        public override void Apply(World world, Entity entity)
        {
            if (!entity.Has<TimerComponent>())
            {
                world.AddComponent(entity, new TimerComponent());
            }

            ref var timer = ref entity.Get<TimerComponent>();
            timer.timer = new Cooldown(totalTime);
            timer.paused = startsPaused;
        }
    }
}