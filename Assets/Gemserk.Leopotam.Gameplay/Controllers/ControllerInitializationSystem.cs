﻿using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Gameplay.Events;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Gemserk.Leopotam.Gameplay.Controllers
{
    public class ControllerInitializationSystem : BaseSystem, IEcsRunSystem, IEntityDestroyedHandler, IEntityCreatedHandler
    {
        readonly EcsFilterInject<Inc<ControllerComponent>> controllerFilter = default;
        readonly EcsPoolInject<ControllerComponent> controllerComponents = default;

        private readonly List<IController> controllersList = new List<IController>();
        
        public void OnEntityCreated(World world, Entity entity)
        {
            if (world.HasComponent<ControllerComponent>(entity))
            {
                ref var controllerComponent = ref world.GetComponent<ControllerComponent>(entity);
                controllerComponent.instance = Instantiate(controllerComponent.prefab);
                controllerComponent.instance.name = $"~{controllerComponent.prefab.name}";
                controllerComponent.controllers = new List<IController>();
                controllerComponent.instance.GetComponentsInChildren(controllerComponent.controllers);
            }
        }
        
        public void OnEntityDestroyed(World world, Entity destroyedEntity)
        {
            foreach (var entity in controllerFilter.Value)
            {
                ref var controllerComponent = ref controllerComponents.Value.Get(entity);
                
                if (!controllerComponent.intialized)
                    continue;
                
                foreach (var controller in controllerComponent.controllers)
                {
                    if (controller is IEntityDestroyed onEntityDestroyed)
                    {
                        controller.Bind(world, world.GetEntity(entity));
                        onEntityDestroyed.OnEntityDestroyed(destroyedEntity);
                    }
                }
            }
            
            if (world.HasComponent<ControllerComponent>(destroyedEntity))
            {
                var controllerComponent = world.GetComponent<ControllerComponent>(destroyedEntity);
                if (controllerComponent.instance != null)
                {
                    GameObject.Destroy(controllerComponent.instance);
                }

                controllerComponent.instance = null;
                controllerComponent.controllers.Clear();
            }
        }

        public void Run(EcsSystems systems)
        {
            var configurations = world.GetComponents<ConfigurationComponent>();
            
            foreach (var entity in world.GetFilter<ControllerComponent>().Inc<ConfigurationComponent>().End())
            {
                ref var controllerComponent = ref controllerComponents.Value.Get(entity);
                var configuration = configurations.Get(entity);

                controllerComponent.onConfigurationPending =
                    controllerComponent.configurationVersion != configuration.configuredVersion;
                controllerComponent.configurationVersion = configuration.configuredVersion;
            }

            foreach (var entity in controllerFilter.Value)
            {
                ref var controllerComponent = ref controllerComponents.Value.Get(entity);
                
                controllersList.Clear();
                controllersList.AddRange(controllerComponent.controllers);
                
                foreach (var controller in controllersList)
                {
                    controller.Bind(world, world.GetEntity(entity));

                    if (!controllerComponent.intialized && controller is IInit init)
                    {
                        init.OnInit();
                    }
                    
                    if (controllerComponent.onConfigurationPending && controller is IConfigurable configurable)
                    {
                        configurable.OnConfigured();
                    }
                }
                
                controllerComponent.intialized = true;
                controllerComponent.onConfigurationPending = false;
            }
        }


    }
}