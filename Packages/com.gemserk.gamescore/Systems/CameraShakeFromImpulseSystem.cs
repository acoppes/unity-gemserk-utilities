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
        readonly EcsFilterInject<Inc<CameraImpulseComponent, LookingDirection>, Exc<DisabledComponent>> lookingDirectionFilter = default;
        readonly EcsFilterInject<Inc<CameraImpulseComponent>, Exc<DisabledComponent>> filter = default;
        // readonly EcsFilterInject<Inc<CameraImpulseComponent, LookingDirection>, Exc<DisabledComponent>> filter = default;
        readonly EcsFilterInject<Inc<CameraImpulseComponent, DestroyableComponent>, Exc<DisabledComponent>> destroyables = default;

        public void Run(EcsSystems systems)
        {
            foreach (var e in lookingDirectionFilter.Value)
            {
                ref var cameraImpulse = ref lookingDirectionFilter.Pools.Inc1.Get(e);
                ref var lookingDirection = ref lookingDirectionFilter.Pools.Inc2.Get(e);

                if (cameraImpulse.directionSource == CameraImpulseComponent.DirectionSource.FromLookingDirection)
                {
                    cameraImpulse.direction = lookingDirection.value;
                }
            }
            
            foreach (var e in filter.Value)
            {
                ref var cameraImpulse = ref filter.Pools.Inc1.Get(e);

                if (cameraImpulse.framesToGenerateImpulse <= 0)
                    continue;
                
                if (cameraImpulse.directionSource == CameraImpulseComponent.DirectionSource.FromImpulseSource)
                {
                    // cameraImpulse.direction = cameraImpulse.impulseSource.DefaultVelocity;
                    cameraImpulse.impulseSource.GenerateImpulse(cameraImpulse.force);
                } else if (cameraImpulse.directionSource == CameraImpulseComponent.DirectionSource.Random)
                {
                    cameraImpulse.direction = Random.insideUnitCircle.normalized;
                    cameraImpulse.impulseSource.GenerateImpulse(cameraImpulse.direction * cameraImpulse.force);
                } else if (cameraImpulse.directionSource == CameraImpulseComponent.DirectionSource.FromLookingDirection)
                {
                    cameraImpulse.impulseSource.GenerateImpulse(cameraImpulse.direction.normalized * cameraImpulse.force);
                }
                
                // cameraImpulse.impulseSource.GenerateImpulse(cameraImpulse.direction.normalized * cameraImpulse.force);

                cameraImpulse.framesToGenerateImpulse--;
            }
            
            foreach (var e in destroyables.Value)
            {
                ref var cameraImpulse = ref filter.Pools.Inc1.Get(e);
                ref var destroyable = ref destroyables.Pools.Inc2.Get(e);

                if (cameraImpulse.framesToRemove <= 0)
                {
                    destroyable.destroy = true;
                }
                
                cameraImpulse.framesToRemove--;
            }
        }
    }
}