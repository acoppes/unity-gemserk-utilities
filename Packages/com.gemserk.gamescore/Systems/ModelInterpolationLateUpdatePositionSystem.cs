using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class ModelInterpolationLateUpdatePositionSystem : BaseSystem, IEcsRunSystem
    {
        // this one runs in the fixed update to store previous position.
        
        readonly EcsFilterInject<Inc<ModelInterpolationComponent>, Exc<DisabledComponent>> filter = default;
        
        public void Run(EcsSystems systems)
        {
            var fixedDt = Time.fixedDeltaTime;
            
            foreach (var entity in filter.Value)
            {
                ref var interpolationComponent = ref filter.Pools.Inc1.Get(entity);

                if (!interpolationComponent.disabled)
                {
                    interpolationComponent.time += dt;
                    interpolationComponent.t = Mathf.Clamp01(interpolationComponent.time / fixedDt);
                }
                else
                {
                    interpolationComponent.t = 1;
                }

                
                // Debug.Log($"{entity}: {dt}, {interpolationComponent.time}, {interpolationComponent.t}");
            }
        }
    }
}