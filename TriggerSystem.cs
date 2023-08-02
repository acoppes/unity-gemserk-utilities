using System.Collections.Generic;
using UnityEngine;

namespace Gemserk.Triggers
{
    public class TriggerSystem : MonoBehaviour
    {
        public enum UpdateType
        {
            FixedUpdate = 0,
            Update = 1,
            LateUpdate = 2,
            Script = 3
        }

        public UpdateType updateType = UpdateType.Update;
        
        private List<ITrigger> triggers = new ();

        private void Awake()
        {
            GetComponentsInChildren(triggers);
        }

        private void FixedUpdate()
        {
            if (updateType != UpdateType.FixedUpdate)
                return;
            UpdateTriggers();
        }
        
        private void Update()
        {
            if (updateType != UpdateType.Update)
                return;
            UpdateTriggers();
        }

        private void LateUpdate()
        {
            if (updateType != UpdateType.LateUpdate) 
                return;
            UpdateTriggers();
        }
        
        public void UpdateTriggers()
        {
            foreach (var trigger in triggers)
            {
                if (trigger.State == ITrigger.ExecutionState.PendingExecution)
                {
                    trigger.StartExecution();
                }

                if (trigger.State == ITrigger.ExecutionState.Executing)
                {
                    if (trigger.Execute() == ITrigger.ExecutionResult.Completed)
                    {
                        trigger.CompleteCurrentExecution();
                    }
                }
            }
        }
    }
}