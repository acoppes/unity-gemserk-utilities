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
        void OnGlobalSignal(World world, Entity entity, ISignal signal, object userData);
    }

    public struct OnSignalEventsComponent : IEntityComponent
    {
        public List<ISignal> signals;
    }
    
    public class OnSignalEventControllerSystem : BaseSystem, IEcsDestroySystem, IEcsRunSystem
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
            public OnSignalEventControllerSystem system;
            public ISignal signal;

            public LocalSignalHandler(ISignal signal, OnSignalEventControllerSystem system)
            {
                this.system = system;
                this.signal = signal;
                signal.Register(OnSignalEvent);
            }

            public void Dispose()
            {
                signal?.Unregister(OnSignalEvent);
                signal = null;
                system = null;
            }

            private void OnSignalEvent(object userData)
            {
                system.OnSignalEvent(signal, userData);
            }
        }
        
        readonly EcsFilterInject<Inc<ControllerComponent>, Exc<DisabledComponent, ControllerSignalHandlerComponent, ControllerSignalHandlerCheckedComponent>> controllers = default;
        readonly EcsFilterInject<Inc<ControllerSignalHandlerComponent>, Exc<DisabledComponent>> controllersWithHandler = default;
        readonly EcsFilterInject<Inc<OnSignalEventsComponent>, Exc<DisabledComponent>> onSignalRegisterFilter = default;

        private readonly List<ISignalControllerEvent> signalHandlers = new ();

        private readonly List<LocalSignalHandler> localSignalHandlers = new List<LocalSignalHandler>();
        
        
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

        public void Run(EcsSystems systems)
        {
            foreach (var entity in onSignalRegisterFilter.Value)
            {
                ref var signalEventsComponent = ref onSignalRegisterFilter.Pools.Inc1.Get(entity);
                
                if (signalEventsComponent.signals != null)
                {
                    foreach (var signal in signalEventsComponent.signals)
                    {
                        var registered = false;
                        foreach (var signalHandler in localSignalHandlers)
                        {
                            if (signalHandler.signal == signal)
                            {
                                registered = true;
                                break;
                            }
                        }

                        if (!registered)
                        {
                            localSignalHandlers.Add(new LocalSignalHandler(signal, this));
                        }
                    }
                }
                
                onSignalRegisterFilter.Pools.Inc1.Del(entity);
            }
            
            if (localSignalHandlers.Count == 0)
            {
                return;
            }
            
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