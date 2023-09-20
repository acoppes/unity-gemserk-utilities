using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;

namespace Game.Systems
{
    public class TimerSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var deltaTime = this.dt;
            var timerComponents = world.GetComponents<TimerComponent>();
            
            foreach (var entity in world.GetFilter<TimerComponent>()
                         .Exc<DisabledComponent>()
                         .End())
            {
                ref var timerComponent = ref timerComponents.Get(entity);
                timerComponent.timer.Increase(deltaTime);
            }
        }
    }
}