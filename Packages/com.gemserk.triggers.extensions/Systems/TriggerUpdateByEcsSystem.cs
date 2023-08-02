using System;
using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace Gemserk.Triggers.Systems
{
    public class TriggerUpdateByEcsSystem : BaseSystem, IEcsRunSystem, IEcsInitSystem
    {
        [NonSerialized]
        private readonly List<TriggerSystem> triggerContainers = new ();

        public void Init(EcsSystems systems)
        {
            triggerContainers.AddRange(FindObjectsByType<TriggerSystem>(FindObjectsInactive.Exclude, 
                FindObjectsSortMode.None));
        }
        
        public void Run(EcsSystems systems)
        {
            for (var i = 0; i < triggerContainers.Count; i++)
            {
                var triggerContainer = triggerContainers[i];
                if (triggerContainer.updateType != TriggerSystem.UpdateType.Script)
                    continue;
                triggerContainer.UpdateTriggers();
            }
        }


    }
}