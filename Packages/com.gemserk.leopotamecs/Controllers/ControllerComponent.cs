using System.Collections.Generic;
using Gemserk.Leopotam.Ecs.Events;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Controllers
{
    public struct ControllerComponent : IEntityComponent
    {
        public GameObject prefab;
        public GameObject instance;
        
        public List<IController> controllers;
        public bool intialized;

        public bool onConfigurationPending;
        public int configurationVersion;
        
        // TODO cache each interface cast

        public List<IStateChanged> stateChangedListeners;
    }
}