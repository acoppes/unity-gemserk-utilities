using System;
using Gemserk.Utilities;
using UnityEngine;

namespace Gemserk.Triggers
{
    public class TriggerObject : MonoBehaviour, ITrigger
    {
        public enum ExecutionType
        {
            Disabled = 0,
            Once = 1,
            More = 2
        }
        
        [NonSerialized]
        public readonly Trigger trigger = new ();
        
        private Transform eventsParent;
        private Transform conditionsParent;
        private Transform actionsParent;

        // Max times the trigger executes. Use 0 or negative to ignore.
        public ExecutionType executionType;
        public int maxExecutions;
        
        public ITrigger.ExecutionState State
        {
            get
            {
                if (!gameObject.activeInHierarchy)
                {
                    return ITrigger.ExecutionState.Waiting;
                }

                return trigger.State;
            }
        }
        
        private void Awake()
        {
            if (executionType == ExecutionType.Disabled)
                trigger.maxExecutionTimes = 0;
            else if (executionType == ExecutionType.Once)
            {
                trigger.maxExecutionTimes = 1;
            } else if (executionType == ExecutionType.More)
            {
                trigger.maxExecutionTimes = maxExecutions;
            }
            
            eventsParent = transform.Find("Events");
            conditionsParent = transform.Find("Conditions");
            actionsParent = transform.Find("Actions");

            eventsParent.gameObject.GetComponentsInChildrenDepth1(true, true, trigger.events);
            conditionsParent.gameObject.GetComponentsInChildrenDepth1(true, true, trigger.conditions);
            actionsParent.gameObject.GetComponentsInChildrenDepth1(true, true, trigger.actions);
        }

        public ITrigger.ExecutionResult Execute()
        {
            return trigger.Execute();
        }

        public void ForceQueueExecution(object activator = null)
        {
            trigger.ForceQueueExecution(activator);
        }
        
        public void QueueExecution(object activator = null)
        {
            trigger.QueueExecution(activator);
        }

        public void StartExecution()
        {
            trigger.StartExecution();
        }

        public void CompleteCurrentExecution()
        {
            trigger.CompleteCurrentExecution();
        }

        public bool IsDisabled()
        {
            return !isActiveAndEnabled && !trigger.IsDisabled();
        }
    }
}