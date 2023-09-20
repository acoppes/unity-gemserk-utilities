using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities.Signals;
using Leopotam.EcsLite;
using UnityEngine;

namespace Game.Systems
{
    public class SpawnSignalSystem : BaseSystem, IEcsRunSystem
    {
        [SerializeField]
        private SignalAsset onSpawnedSignal;
        
        public void Run(EcsSystems systems)
        {
            // auto disable?
            if (onSpawnedSignal == null)
            {
                return;
            }
            
            foreach (var entity in world
                         .GetFilter<SpawnSignalComponent>()
                         .Exc<DisabledComponent>()
                         .End())
            {
                var worldEntity = world.GetEntity(entity);
                
                onSpawnedSignal.Signal(worldEntity);
                world.RemoveComponent<SpawnSignalComponent>(worldEntity);
            }
        }
    }
}