using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using MyBox;
using UnityEngine;

namespace Game.Systems
{
    public class OffScreenEntityDisableSystem : BaseSystem, IEcsRunSystem
    {
        public Camera worldCamera;

        public void Run(EcsSystems systems)
        {
            if (worldCamera == null)
                return;
            
            var positionComponents = world.GetComponents<PositionComponent>();
            var offScreenComponents = world.GetComponents<OffScreenDisableComponent>();

            var cameraBounds = worldCamera.GetBounds();
            cameraBounds.center = cameraBounds.center.SetZ(0);
            cameraBounds.extents = cameraBounds.extents.SetZ(100000);
            
            // temporary fix since there are no bounds configured in models yet to avoid 
            // objects being activated after already inside the screen
            cameraBounds.Expand(1);

            var filter = world.GetFilter<OffScreenDisableComponent>()
                .Inc<PositionComponent>()
                .Exc<DisabledComponent>()
                .End();
            
            foreach (var entity in filter)
            {
                var positionComponent = positionComponents.Get(entity);
                ref var offScreenDisableComponent = ref offScreenComponents.Get(entity);

                var objectBounds = offScreenDisableComponent.bounds;
                objectBounds.center = GamePerspective.ConvertFromWorld(positionComponent.value);
                
                var insideCamera = cameraBounds.Intersects(objectBounds);
                
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
            
            filter = world.GetFilter<OffScreenDisableComponent>()
                .Inc<PositionComponent>()
                .Inc<DisabledComponent>()
                .End();
            
            foreach (var entity in filter)
            {
                var positionComponent = positionComponents.Get(entity);
                var offScreenDisableComponent = offScreenComponents.Get(entity);

                var objectBounds = offScreenDisableComponent.bounds;
                objectBounds.center = GamePerspective.ConvertFromWorld(positionComponent.value);
                
                var insideCamera = cameraBounds.Intersects(objectBounds);
                
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