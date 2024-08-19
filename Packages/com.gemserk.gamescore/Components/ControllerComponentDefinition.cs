using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using UnityEngine;

namespace Game.Components
{
    public class ControllerComponentDefinition : ComponentDefinitionBase
    {
        public GameObject controllerObject;
        public bool sharedInstance;

        public bool hasStateController = true;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new ControllerComponent
            {
                prefab = controllerObject,
                sharedInstance = sharedInstance
            });
            
            if (hasStateController)
            {
                world.AddComponent(entity, new ActiveControllerComponent()
                {
                    activeController = null
                });
            }
        }
    }
}