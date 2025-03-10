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
        public static float globalIntensity = 1f;
        
        readonly EcsFilterInject<Inc<CameraImpulseComponent, LookingDirection>, Exc<DisabledComponent>> lookingDirectionFilter = default;
        readonly EcsFilterInject<Inc<CameraImpulseComponent, PositionComponent>, Exc<DisabledComponent>> filter = default;
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
                var position = filter.Pools.Inc2.Get(e);

                if (cameraImpulse.framesToGenerateImpulse <= 0)
                    continue;

                var force = cameraImpulse.force * globalIntensity;
                
                if (cameraImpulse.directionSource == CameraImpulseComponent.DirectionSource.FromImpulseSource)
                {
                    // cameraImpulse.direction = cameraImpulse.impulseSource.DefaultVelocity;
                    cameraImpulse.impulseSource.GenerateImpulseAtPositionWithVelocity(position.value, 
                        cameraImpulse.impulseSource.DefaultVelocity * force);
                } else if (cameraImpulse.directionSource == CameraImpulseComponent.DirectionSource.Random)
                {
                    cameraImpulse.direction = Random.insideUnitCircle.normalized;
                    cameraImpulse.impulseSource.GenerateImpulseAtPositionWithVelocity(position.value, 
                        cameraImpulse.direction * force);
                } else if (cameraImpulse.directionSource == CameraImpulseComponent.DirectionSource.FromLookingDirection)
                {
                    cameraImpulse.impulseSource.GenerateImpulseAtPositionWithVelocity(position.value, cameraImpulse.direction.normalized * force);
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