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
        }


    }
}