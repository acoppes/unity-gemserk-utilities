using System;
using Gemserk.Utilities;
using UnityEngine;

namespace Gemserk.Triggers
{
    public class TriggerObject : MonoBehaviour, ITrigger
    {
        [NonSerialized]
        public readonly Trigger trigger = new ();
        
        private Transform eventsParent;
        private Transform conditionsParent;
        private Transform actionsParent;

        // Max times the trigger executes. Use 0 or negative to ignore.
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
            trigger.maxExecutionTimes = maxExecutions;
            
            eventsParent = transform.Find("Events");
            conditionsParent = transform.Find("Conditions");
            actionsParent = transform.Find("Actions");

            eventsParent.gameObject.GetComponentsInChildrenDepth1(false, true, trigger.events);
            conditionsParent.gameObject.GetComponentsInChildrenDepth1(false, true, trigger.conditions);
            actionsParent.gameObject.GetComponentsInChildrenDepth1(false, true, trigger.actions);
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
    }
}