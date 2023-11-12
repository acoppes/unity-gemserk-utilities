using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MyBox;
using UnityEngine;

namespace Game.Systems
{
    public class OffScreenEntityDisableSystem : BaseSystem, IEcsRunSystem, IEcsInitSystem
    {
        readonly EcsFilterInject<Inc<PositionComponent, OffScreenDisableComponent>, Exc<DisabledComponent>> offscreenFilter = default;
        readonly EcsFilterInject<Inc<PositionComponent, OffScreenDisableComponent, DisabledComponent>> 
            disabledFilter = default;
        
        public Camera worldCamera;
        
        public void Init(EcsSystems systems)
        {
            if (worldCamera == null)
            {
                worldCamera = GameObject.FindWithTag("WorldCamera").GetComponent<Camera>();
            }
        }

        public void Run(EcsSystems systems)
        {
            if (worldCamera == null)
                return;

            var cameraBounds = worldCamera.GetBounds();
            cameraBounds.center = cameraBounds.center.SetZ(0);
            cameraBounds.extents = cameraBounds.extents.SetZ(100000);
            
            // temporary fix since there are no bounds configured in models yet to avoid 
            // objects being activated after already inside the screen
            cameraBounds.Expand(1);

            var cameraMax = cameraBounds.max;
            var cameraMin = cameraBounds.min;

            foreach (var entity in offscreenFilter.Value)
            {
                var positionComponent = offscreenFilter.Pools.Inc1.Get(entity);
                ref var offScreenDisableComponent = ref offscreenFilter.Pools.Inc2.Get(entity);

                var position = positionComponent.value;
                
                var objectBounds = offScreenDisableComponent.bounds;
                objectBounds.center = positionComponent.value;

                if (positionComponent.type == 0)
                {
                    objectBounds.center = GamePerspective.ConvertFromWorld(positionComponent.value);
                    position = GamePerspective.ConvertFromWorld(positionComponent.value);
                }
                
                // var insideCamera = cameraBounds.Intersects(objectBounds);
                
                // don't check z and we don't care about double precision


                var insideCamera = true;

                if (offScreenDisableComponent.boundsType == OffScreenDisableComponent.BoundsType.Fixed)
                {
                    var objectBoundsMax = objectBounds.max;
                    var objectBoundsMin = objectBounds.min;
                    insideCamera = cameraMin.x <= objectBoundsMax.x && cameraMax.x >= objectBoundsMin.x && cameraMin.y <= objectBoundsMax.y && cameraMax.y >= objectBoundsMin.y;
                } else if (offScreenDisableComponent.boundsType == OffScreenDisableComponent.BoundsType.NoBounds)
                {
                    insideCamera = cameraMin.x <= position.x && cameraMax.x >= position.x && cameraMin.y <= position.y && cameraMax.y >= position.y;
                }
                
                if (!insideCamera)
                {
                    world.AddComponent(world.GetEntity(entity), new DisabledComponent());
                    // offScreenDisableComponent.disableCount++;
                }
                else
                {
                    if (offScreenDisableComponent.disableType == OffScreenDisableComponent.DisableType.FirstTimeOnly)
                    {
                        world.RemoveComponent<OffScreenDisableComponent>(world.GetEntity(entity));
                    }
                }
            }
            
            foreach (var entity in disabledFilter.Value)
            {
                var positionComponent = disabledFilter.Pools.Inc1.Get(entity);
                var offScreenDisableComponent = disabledFilter.Pools.Inc2.Get(entity);

                var position = positionComponent.value;
                
                var objectBounds = offScreenDisableComponent.bounds;
                objectBounds.center = positionComponent.value;

                if (positionComponent.type == 0)
                {
                    objectBounds.center = GamePerspective.ConvertFromWorld(positionComponent.value);
                    position = GamePerspective.ConvertFromWorld(positionComponent.value);
                }
                
                var insideCamera = true;

                if (offScreenDisableComponent.boundsType == OffScreenDisableComponent.BoundsType.Fixed)
                {
                    var objectBoundsMax = objectBounds.max;
                    var objectBoundsMin = objectBounds.min;
                    insideCamera = cameraMin.x <= objectBoundsMax.x && cameraMax.x >= objectBoundsMin.x && cameraMin.y <= objectBoundsMax.y && cameraMax.y >= objectBoundsMin.y;
                } else if (offScreenDisableComponent.boundsType == OffScreenDisableComponent.BoundsType.NoBounds)
                {
                    insideCamera = cameraMin.x <= position.x && cameraMax.x >= position.x && cameraMin.y <= position.y && cameraMax.y >= position.y;
                }
                
                if (insideCamera)
                {
                    world.RemoveComponent<DisabledComponent>(world.GetEntity(entity));

                    if (offScreenDisableComponent.disableType == OffScreenDisableComponent.DisableType.FirstTimeOnly)
                    {
                        world.RemoveComponent<OffScreenDisableComponent>(world.GetEntity(entity));
                    }
                }
            }
        }


    }
}