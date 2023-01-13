using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gemserk.Actions
{
    public class TriggerSystem : MonoBehaviour
    {
        private List<ITrigger> triggers = new List<ITrigger>();

        private void Awake()
        {
            GetComponentsInChildren(triggers);
        }

        private void Update()
        {
            foreach (var trigger in triggers)
            {
                if (trigger.State == ITrigger.ExecutionState.Waiting)
                {
                    if (trigger.Evaluate())
                    {
                        trigger.StartExecution();
                    }
                }

                if (trigger.State == ITrigger.ExecutionState.Executing)
                {
                    if (trigger.Execute() == ITrigger.ExecutionResult.Completed)
                    {
                        trigger.StopExecution();
                    }
                }
            }
        }
    }
}