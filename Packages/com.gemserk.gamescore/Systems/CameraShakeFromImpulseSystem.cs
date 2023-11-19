using Game.Components;
using Game.Definitions;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class CameraShakeFromImpulseSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<CameraImpulseComponent>, Exc<DisabledComponent>> filter = default;
        readonly EcsFilterInject<Inc<CameraImpulseComponent, DestroyableComponent>, Exc<DisabledComponent>> destroyables = default;

        public void Run(EcsSystems systems)
        {
            foreach (var e in filter.Value)
            {
                ref var cameraImpulse = ref filter.Pools.Inc1.Get(e);
                cameraImpulse.impulseSource.GenerateImpulse(cameraImpulse.force);
            }
            
            foreach (var e in destroyables.Value)
            {
                ref var destroyable = ref destroyables.Pools.Inc2.Get(e);
                destroyable.destroy = true;
            }
        }
    }
}