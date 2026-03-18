using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gemserk.Triggers
{
    public class TriggerSystemExecutor : ITriggerSystem
    {
        public readonly List<ITrigger> triggers = new ();
        
        public void Execute()
        {
            foreach (var trigger in triggers)
            {
                if (trigger.IsDisabled())
                {
                    continue;
                }
                
                if (trigger.State == ITrigger.ExecutionState.PendingExecution)
                {
                    trigger.StartExecution();
                }

                if (trigger.State == ITrigger.ExecutionState.Executing)
                {
                    try
                    {
                        var result = trigger.Execute();
                        if (result == ITrigger.ExecutionResult.Completed ||
                            result == ITrigger.ExecutionResult.Interrupt)
                        {
                            trigger.CompleteCurrentExecution();
                        }
                    }
                    catch (Exception e)
                    {
                        if (trigger is ITriggerErrorData triggerErrorData)
                        {
                            triggerErrorData.LogError(e);
                        }
                        else
                        {
                            Debug.LogException(e);
                        }
                    }
                }
            }
        }
    }
}