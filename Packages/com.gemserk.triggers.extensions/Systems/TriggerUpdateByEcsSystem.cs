using System;
using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Gemserk.Triggers.Systems
{
    public struct TriggerSystemComponent : IEntityComponent
    {
        public ITriggerSystem triggerSystem;
    }
    
    public class TriggerUpdateByEcsSystem : BaseSystem, IEcsRunSystem, IEcsInitSystem
    {
        [NonSerialized]
        private readonly List<TriggerSystem> triggerContainers = new ();
        
        readonly EcsFilterInject<Inc<TriggerSystemComponent>, Exc<DisabledComponent>> triggerSystemsFilter = default;

        public void Init(EcsSystems systems)
        {
            var triggerSystems = FindObjectsByType<TriggerSystem>(FindObjectsInactive.Exclude);
            foreach (var triggerSystem in triggerSystems)
            {
                if (triggerSystem.GetComponent<AutoCreateTriggerSystemEntity>())
                {
                    continue;
                }
                triggerContainers.Add(triggerSystem);
            }
        }
        
        public void Run(EcsSystems systems)
        {
            for (var i = 0; i < triggerContainers.Count; i++)
            {
                var triggerContainer = triggerContainers[i];
                if (triggerContainer.updateType != TriggerSystem.UpdateType.Script)
                    continue;
                triggerContainer.Execute();
            }
            
            foreach (var e in triggerSystemsFilter.Value)
            {
                ref var triggerSystemComponent = ref triggerSystemsFilter.Pools.Inc1.Get(e);
                triggerSystemComponent.triggerSystem.Execute();
            }
        }
    }
}