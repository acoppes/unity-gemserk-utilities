using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities.Signals;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class TimerSystem : BaseSystem, IEcsRunSystem
    {
        public SignalAsset onTimerCompletedSignal;
        
        readonly EcsFilterInject<Inc<TimerComponent>, Exc<DisabledComponent>> timers = default;
        
        public void Run(EcsSystems systems)
        {
            var deltaTime = dt;
            
            foreach (var e in timers.Value)
            {
                ref var timerComponent = ref timers.Pools.Inc1.Get(e);

                if (timerComponent.paused)
                {
                    continue;
                }

                timerComponent.wasReady = timerComponent.timer.IsReady;
                
                timerComponent.timer.Increase(deltaTime);

                if (timerComponent.timer.IsReady && !timerComponent.wasReady)
                {
                    onTimerCompletedSignal.Signal(world.GetEntity(e));
                } 
            }
        }
    }
}