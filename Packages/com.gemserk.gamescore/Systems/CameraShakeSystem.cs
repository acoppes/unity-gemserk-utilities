using Game.Components;
using Game.Definitions;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class CameraShakeSystem : BaseSystem, IEcsRunSystem, IEcsInitSystem
    {
        readonly EcsFilterInject<Inc<CameraShakeProvider>, Exc<DisabledComponent>> filter = default;
        
        private GameObject worldCamera;
        
        public void Init(EcsSystems systems)
        {
            worldCamera = GameObject.FindWithTag("WorldCamera");
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in filter.Value)
            {
                ref var cameraShakeProvider = ref filter.Pools.Inc1.Get(entity);

                if (cameraShakeProvider.shake != null)
                {
                    StartCoroutine(CameraShake.Shake(cameraShakeProvider.shake, worldCamera.transform));
                    cameraShakeProvider.shake = null;
                }
            }
        }
        
        
    }
}