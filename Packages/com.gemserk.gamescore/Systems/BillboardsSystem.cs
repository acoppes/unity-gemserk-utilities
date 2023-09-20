using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MyBox;
using UnityEngine;
using Vertx.Debugging;

namespace Game.Systems
{
    public class BillboardsSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<BillboardComponent, PositionComponent>, Exc<DisabledComponent>> filter = default;
        readonly EcsFilterInject<Inc<BillboardComponent, ModelComponent>, Exc<DisabledComponent>> modelFilter = default;

        public void Run(EcsSystems systems)
        {
            if (world.TryGetSingletonEntity<CameraComponent, GameObjectComponent>(out var cameraEntity))
            {
                var cameraGameObject = cameraEntity.Get<GameObjectComponent>();

                foreach (var e in filter.Value)
                {
                    ref var billboard = ref filter.Pools.Inc1.Get(e);
                    var position = filter.Pools.Inc2.Get(e);

                    // TODO: test move camera and also rotate?
                    var center = Vector3.zero;

                    var cameraPosition = cameraGameObject.gameObject.transform.position;
                    billboard.cameraDirection = (cameraPosition - center).normalized;
                    billboard.cameraAngle = cameraGameObject.gameObject.transform.localEulerAngles.x;

                    billboard.lookAtPosition = position.value - billboard.cameraDirection;
                }
            }
            
            // TODO: rotate model with given direction?
            
            foreach (var e in modelFilter.Value)
            {
                ref var billboard = ref modelFilter.Pools.Inc1.Get(e);
                ref var model = ref modelFilter.Pools.Inc2.Get(e);
                
                // float scale = 1f / (float)Math.Cos(camera.Rotation.X); (camera rotation is in radians) 

                // var t = model.instance.model;
                // t.LookAt(billboard.lookAtPosition);

                model.instance.transform.localScale = model.instance.model.localScale.SetY(1.0f / Mathf.Cos(billboard.cameraAngle * Mathf.Deg2Rad));
            }
            
            foreach (var e in filter.Value)
            {
                ref var billboard = ref filter.Pools.Inc1.Get(e);
                var position = filter.Pools.Inc2.Get(e);
                
                D.raw(new Shape.Line(position.value, position.value + billboard.cameraDirection*3));
            }
        }


    }
}