using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace Game.Systems
{
    public class PlayerIndicatorModelSystem : BaseSystem, IEcsRunSystem
    {
        public Color[] playerColors;
        
        public void Run(EcsSystems systems)
        {
            var modelComponents = world.GetComponents<ModelComponent>();
            var playerInputComponents = world.GetComponents<PlayerInputComponent>();

            var controlledCount = 0;
            
            foreach (var _ in world.GetFilter<PlayerInputComponent>().End())
            {
                controlledCount++;
                
                // var playerInputComponent = playerInputComponents.Get(entity);
                // if (playerInputComponent.isControlled)
                // {
                //     controlledCount++;
                // }
            }
            
            foreach (var entity in world.GetFilter<ModelComponent>().Inc<PlayerInputComponent>().End())
            {
                var modelComponent = modelComponents.Get(entity);
                var playerInputComponent = playerInputComponents.Get(entity);

                // if (modelComponent.instance.playerIndicator != null)
                // {
                //     modelComponent.instance.playerIndicator.enabled = playerInputComponent.isControlled 
                //                                                       && controlledCount > 1;
                //     modelComponent.instance.playerIndicator.color = playerColors[playerInputComponent.playerInput];
                //     
                //     if (playerInputComponent.isControlled)
                //     {
                //         controlledCount++;
                //     }
                // }
            }
        }
    }
}