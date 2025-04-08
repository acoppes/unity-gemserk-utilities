using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities.Signals;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class SpawnSignalSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<SpawnSignalComponent>, Exc<DisabledComponent>> 
            spawnSignals = default;
        
        [SerializeField]
        private SignalAsset onSpawnedSignal;
        
        public void Run(EcsSystems systems)
        {
            // auto disable?
            if (!onSpawnedSignal)
            {
                return;
            }

            foreach (var e in spawnSignals.Value)
            {
                var worldEntity = world.GetEntity(e);
                onSpawnedSignal.Signal(worldEntity);
                spawnSignals.Pools.Inc1.Del(e);
            }
            
            // foreach (var entity in world
            //              .GetFilter<SpawnSignalComponent>()
            //              .Exc<DisabledComponent>()
            //              .End())
            // {
            //     var worldEntity = world.GetEntity(entity);
            //     
            //     onSpawnedSignal.Signal(worldEntity);
            //     world.RemoveComponent<SpawnSignalComponent>(worldEntity);
            // }
        }
    }
}