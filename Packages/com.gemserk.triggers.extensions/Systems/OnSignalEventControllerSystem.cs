using System;
using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;
using Gemserk.Utilities.Signals;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Gemserk.Triggers.Systems
{
    public interface ISignalControllerEvent : IControllerEvent
    {
        void OnGlobalSignal(World world, Entity entity, ISignal sourceSignal, object userData);
    }
    
    public class OnSignalEventControllerSystem : BaseSystem, IEcsInitSystem, IEcsDestroySystem, IEcsRunSystem
    {
        private struct ControllerSignalHandlerComponent : IEntityComponent
        {
            public List<ISignalControllerEvent> signalEventHandlers;
        }
        
        private struct ControllerSignalHandlerCheckedComponent : IEntityComponent
        {
            
        }

        private class LocalSignalHandler : IDisposable
        {
            private OnSignalEventControllerSystem system;
            private SignalAsset signalAsset;

            public LocalSignalHandler(SignalAsset signalAsset, OnSignalEventControllerSystem system)
            {
                this.signalAsset = signalAsset;
                this.system = system;
                this.signalAsset.Register(OnSignalEvent);
            }

            public void Dispose()
            {
                signalAsset.Unregister(OnSignalEvent);
                signalAsset = null;
                system = null;
            }

            private void OnSignalEvent(object userData)
            {
                system.OnSignalEvent(signalAsset, userData);
            }
        }
        
        readonly EcsFilterInject<Inc<ControllerComponent>, Exc<DisabledComponent, ControllerSignalHandlerComponent, ControllerSignalHandlerCheckedComponent>> controllers = default;
        readonly EcsFilterInject<Inc<ControllerSignalHandlerComponent>, Exc<DisabledComponent>> controllersWithHandler = default;

        private readonly List<ISignalControllerEvent> signalHandlers = new ();

        public List<SignalAsset> signalAssets = new List<SignalAsset>();

        private readonly List<LocalSignalHandler> localSignalHandlers = new List<LocalSignalHandler>();

        public void Init(EcsSystems systems)
        {
            foreach (var signalAsset in signalAssets)
            {
                localSignalHandlers.Add(new LocalSignalHandler(signalAsset, this));
            }
        }
        
        public void Destroy(EcsSystems systems)
        {
            foreach (var localSignalHandler in localSignalHandlers)
            {
                localSignalHandler.Dispose();
            }
            localSignalHandlers.Clear();
        }

        private void OnSignalEvent(ISignal signal, object userData)
        {
            foreach (var e in controllersWithHandler.Value)
            {
                ref var controllerWithHandlers = ref controllersWithHandler.Pools.Inc1.Get(e);
                foreach (var signalEventHandler in controllerWithHandlers.signalEventHandlers)
                {
                    signalEventHandler.OnGlobalSignal(world, world.GetEntity(e), signal, userData);
                }
            }
        }

        // public void OnEntityCreated(World world, Entity entity)
        // {
        //     if (entity.Has<ControllerComponent>() && !entity.Has<ControllerSignalHandlerComponent>())
        //     {
        //         var controllers = entity.Get<ControllerComponent>();
        //         foreach (var controller in controllers.controllers)
        //         {
        //             if (controller is ISignalControllerEvent signalEventHandler)
        //             {
        //                 signalHandlers.Add(signalEventHandler);
        //             }
        //         }
        //
        //         if (signalHandlers.Count > 0)
        //         {
        //             entity.Add(new ControllerSignalHandlerComponent()
        //             {
        //                 signalEventHandlers = new List<ISignalControllerEvent>(signalHandlers)
        //             });
        //             
        //             signalHandlers.Clear();
        //         }
        //     }
        // }
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in controllers.Value)
            {
                ref var controllerComponent = ref controllers.Pools.Inc1.Get(entity);

                if (!controllerComponent.intialized)
                {
                    continue;
                }
                
                foreach (var controller in controllerComponent.controllers)
                {
                    if (controller is ISignalControllerEvent signalEventHandler)
                    {
                        signalHandlers.Add(signalEventHandler);
                    }
                }
                
                if (signalHandlers.Count > 0)
                {
                    world.AddComponent(entity, new ControllerSignalHandlerComponent()
                    {
                        signalEventHandlers = new List<ISignalControllerEvent>(signalHandlers)
                    });
                    signalHandlers.Clear();
                }
                else
                {
                    world.AddComponent(entity, new ControllerSignalHandlerCheckedComponent());
                }
            }
        }
    }
}