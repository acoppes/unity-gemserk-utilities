using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class ActiveControllerSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<ActiveControllerComponent>, Exc<DisabledComponent>> filter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in filter.Value)
            {
                ref var activeController = ref filter.Pools.Inc1.Get(entity);

                if (activeController.activeController != null)
                {
                    activeController.controlledTime += dt;
                    activeController.controlledFrames++;
                    
                    activeController.lastControlledTime = activeController.controlledTime;
                    activeController.lastControlledFrames = activeController.controlledFrames;
                }
                else
                {
                    activeController.controlledTime = 0;
                    activeController.controlledFrames = 0;
                }
            }
        }
    }
}