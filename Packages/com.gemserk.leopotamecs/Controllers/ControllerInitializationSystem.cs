using System.Collections.Generic;
using Gemserk.Leopotam.Ecs.Events;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Controllers
{
    public class ControllerInitializationSystem : BaseSystem, IEcsRunSystem, IEntityDestroyedHandler, 
        IEntityCreatedHandler, IEcsInitSystem
    {
        readonly EcsFilterInject<Inc<ControllerComponent>, Exc<DisabledComponent>> controllerFilter = default;
        readonly EcsPoolInject<ControllerComponent> controllerComponents = default;
        
        private GameObject instancesParent;
        
        private readonly List<IController> controllersList = new List<IController>();
        
        public void Init(EcsSystems systems)
        {
            const string parentGameObjectName = "~Controllers";
            
            if (instancesParent == null)
            {
                instancesParent = GameObject.Find(parentGameObjectName);
            }

            if (instancesParent == null)
            {
                instancesParent = new GameObject(parentGameObjectName);
            }
        }

        public void OnEntityCreated(World world, Entity entity)
        {
            if (world.HasComponent<ControllerComponent>(entity))
            {
                ref var controllerComponent = ref world.GetComponent<ControllerComponent>(entity);
                controllerComponent.instance = Instantiate(controllerComponent.prefab);
                controllerComponent.instance.transform.parent = instancesParent.transform;

                controllerComponent.instance.name = $"{controllerComponent.prefab.name}";
                
                controllerComponent.controllers = new List<IController>();
                controllerComponent.instance.GetComponentsInChildren(controllerComponent.controllers);

                controllerComponent.stateChangedListeners = new List<IStateChanged>();
                controllerComponent.instance.GetComponentsInChildren(controllerComponent.stateChangedListeners);
                
                var entityReference = controllerComponent.instance.AddComponent<EntityReference>();
                entityReference.entity = entity;
            }
        }
        
        public void OnEntityDestroyed(World world, Entity destroyedEntity)
        {
            foreach (var entity in controllerFilter.Value)
            {
                ref var controllerComponent = ref controllerComponents.Value.Get(entity);

                if (controllerComponent.intialized)
                {
                    foreach (var controller in controllerComponent.controllers)
                    {
                        if (controller is IEntityDestroyed onEntityDestroyed)
                        {
                            controller.Bind(world, world.GetEntity(entity));
                            onEntityDestroyed.OnEntityDestroyed(destroyedEntity);
                        }
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
                controllerComponent.stateChangedListeners.Clear();
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