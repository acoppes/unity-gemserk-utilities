using System;
using Gemserk.Utilities;
using UnityEngine;

namespace Gemserk.Triggers
{
    public class TriggerObject : MonoBehaviour, ITrigger
    {
        [NonSerialized]
        public readonly Trigger trigger = new ();

        public ITrigger.ExecutionState State => trigger.State;

        private Transform eventsParent;
        private Transform conditionsParent;
        private Transform actionsParent;
        
        private void Awake()
        {
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