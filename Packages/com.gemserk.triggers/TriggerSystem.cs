using System.Collections.Generic;
using UnityEngine;

namespace Gemserk.Triggers
{
    public class TriggerSystem : MonoBehaviour, ITriggerSystem
    {
        public enum UpdateType
        {
            FixedUpdate = 0,
            Update = 1,
            LateUpdate = 2,
            Script = 3
        }

        public UpdateType updateType = UpdateType.Update;

        private readonly TriggerSystemExecutor triggerSystem = new();

        public List<ITrigger> triggers => triggerSystem.triggers;

        private void Awake()
        {
            GetComponentsInChildren(true, triggerSystem.triggers);
        }

        private void FixedUpdate()
        {
            if (updateType != UpdateType.FixedUpdate)
                return;
            Execute();
        }
        
        private void Update()
        {
            if (updateType != UpdateType.Update)
                return;
            Execute();
        }

        private void LateUpdate()
        {
            if (updateType != UpdateType.LateUpdate) 
                return;
            Execute();
        }
        
        public void Execute()
        {
            triggerSystem.Execute();
        }
    }
}