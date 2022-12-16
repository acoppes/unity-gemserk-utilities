using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Gemserk.Leopotam.Gameplay.Controllers
{
    public class ControllerSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<ControllerComponent>> controllerFilter = default;
        readonly EcsPoolInject<ControllerComponent> controllerComponents = default;

        private readonly List<IController> controllersList = new List<IController>();
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in controllerFilter.Value)
            {
                ref var controllerComponent = ref controllerComponents.Value.Get(entity);
                
                controllersList.Clear();
                controllersList.AddRange(controllerComponent.controllers);
                
                foreach (var controller in controllersList)
                {
                    controller.Bind(world, world.GetEntity(entity));

                    if (controller is IUpdate updateable)
                    {
                        updateable.OnUpdate(Time.deltaTime);
                    }
                }
            }
        }
    }
}