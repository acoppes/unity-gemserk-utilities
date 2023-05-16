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
        readonly EcsFilterInject<Inc<ControllerComponent, ConfigurationComponent>, Exc<DisabledComponent>> configurableControllers = default;
        readonly EcsFilterInject<Inc<ControllerComponent>, Exc<DisabledComponent>> controllerFilter = default;
        
        readonly EcsPoolInject<ConfigurationComponent> configurationComponents = default;
        readonly EcsPoolInject<ControllerComponent> controllerComponents = default;
        
#if GEMSERK_CONTROLLERS_DEBUG && UNITY_EDITOR
        private GameObject instancesParent;
#endif

        private readonly Dictionary<GameObject, GameObject> sharedInstances =
            new Dictionary<GameObject, GameObject>();

        private readonly List<IController> controllersList = new List<IController>();

        public void Init(EcsSystems systems)
        {
#if GEMSERK_CONTROLLERS_DEBUG && UNITY_EDITOR
            const string parentGameObjectName = "~Controllers";
            if (instancesParent == null)
            {
                instancesParent = GameObject.Find(parentGameObjectName);
            }

            if (instancesParent == null)
            {
                instancesParent = new GameObject(parentGameObjectName);
            }
#endif
        }

        private GameObject GetControllerInstance(ControllerComponent controllerComponent)
        {
            if (!controllerComponent.sharedInstance)
            {
                // TODO: use pool but have to have a callback to reset
                
                var instance = Instantiate(controllerComponent.prefab);
                
                #if UNITY_EDITOR
                instance.name = $"{controllerComponent.prefab.name}";
                #endif
                
                return instance;
            }

            if (!sharedInstances.ContainsKey(controllerComponent.prefab))
            {
                var instance = Instantiate(controllerComponent.prefab);
#if GEMSERK_CONTROLLERS_DEBUG && UNITY_EDITOR
                instance.name = $"{controllerComponent.prefab.name}_SharedInstance";
#endif
                sharedInstances[controllerComponent.prefab] = instance;
            }

            return sharedInstances[controllerComponent.prefab];
        }

        public void OnEntityCreated(World world, Entity entity)
        {
            if (world.HasComponent<ControllerComponent>(entity))
            {
                ref var controllerComponent = ref world.GetComponent<ControllerComponent>(entity);
                controllerComponent.instance = GetControllerInstance(controllerComponent);
                
#if GEMSERK_CONTROLLERS_DEBUG && UNITY_EDITOR
                controllerComponent.instance.transform.parent = instancesParent.transform;
              
#endif
                controllerComponent.controllers = new List<IController>();
                controllerComponent.instance.GetComponentsInChildren(controllerComponent.controllers);

                controllerComponent.stateChangedListeners = new List<IStateChanged>();
                controllerComponent.instance.GetComponentsInChildren(controllerComponent.stateChangedListeners);
                
                if (!controllerComponent.sharedInstance)
                {
                    var entityReference = controllerComponent.instance.AddComponent<EntityReference>();
                    entityReference.entity = entity;
                }
            }
        }
        
        public void OnEntityDestroyed(World world, Entity destroyedEntity)
        {
            if (world.HasComponent<ControllerComponent>(destroyedEntity))
            {
                var controllerComponent = world.GetComponent<ControllerComponent>(destroyedEntity);
                
                if (controllerComponent.intialized)
                {
                    foreach (var controller in controllerComponent.controllers)
                    {
                        if (controller is IDestroyed onDestroyed)
                        {
                            onDestroyed.OnDestroyed(world, destroyedEntity);
                        }
                    }
                }
                
                if (controllerComponent.instance != null && !controllerComponent.sharedInstance)
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
            foreach (var entity in configurableControllers.Value)
            {
                ref var controllerComponent = ref controllerComponents.Value.Get(entity);
                var configuration = configurationComponents.Value.Get(entity);

                controllerComponent.onConfigurationPending =
                    controllerComponent.configurationVersion != configuration.configuredVersion;
                controllerComponent.configurationVersion = configuration.configuredVersion;
            }

            foreach (var entity in controllerFilter.Value)
            {
                ref var controllerComponent = ref controllerComponents.Value.Get(entity);

                controllersList.Clear();
                controllersList.AddRange(controllerComponent.controllers);
                
                var worldEntity = world.GetEntity(entity);
                
                foreach (var controller in controllersList)
                {
                    if (!controllerComponent.intialized && controller is IInit init)
                    {
                        init.OnInit(world, worldEntity);
                    }
                    
                    if (controllerComponent.onConfigurationPending && controller is IConfigurable configurable)
                    {
                        configurable.OnConfigured(world, worldEntity);
                    }
                }
                
                controllerComponent.intialized = true;
                controllerComponent.onConfigurationPending = false;
            }
        }
    }
}