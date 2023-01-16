using System.Collections.Generic;
using UnityEngine;

namespace Gemserk.Actions
{
    public class TriggerSystem : MonoBehaviour
    {
        private List<ITrigger> triggers = new ();

        private void Awake()
        {
            GetComponentsInChildren(triggers);
        }

        private void Update()
        {
            foreach (var trigger in triggers)
            {
                if (trigger.State == ITrigger.ExecutionState.PendingExecution)
                {
                    trigger.StartExecution();
                    
                    // if (trigger.Evaluate())
                    // {
                    //     trigger.StartExecution();
                    // }
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