using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Controllers
{
    public class ControllerSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<ControllerComponent>> controllerFilter = default;
        readonly EcsPoolInject<ControllerComponent> controllerComponents = default;

        public void Run(EcsSystems systems)
        {
            foreach (var entity in controllerFilter.Value)
            {
                var controllerComponent = controllerComponents.Value.Get(entity);
                controllerComponent.controller?.OnUpdate(Time.deltaTime, world, entity);
            }
        }
    }
}