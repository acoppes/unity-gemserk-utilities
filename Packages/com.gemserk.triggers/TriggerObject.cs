using System;
using Gemserk.Utilities;
using UnityEngine;

namespace Gemserk.Triggers
{
    public class TriggerObject : MonoBehaviour, ITrigger, ITriggerErrorData
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

        public int GetCalculatedMaxExecutions()
        {
            if (executionType == TriggerObject.ExecutionType.Once)
            {
                return 1;
            }

            if (executionType == TriggerObject.ExecutionType.More)
            {
                return maxExecutions;
            }

            return 0;
        }

        public string Name => gameObject.name;

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

        public void Awake()
        {
            var triggerState = trigger.GetTriggerState();
            
            if (executionType == ExecutionType.Disabled)
                triggerState.maxExecutionTimes = 0;
            else if (executionType == ExecutionType.Once)
            {
                triggerState.maxExecutionTimes = 1;
            } else if (executionType == ExecutionType.More)
            {
                triggerState.maxExecutionTimes = maxExecutions;
            }
            
            eventsParent = transform.Find("Events");
            conditionsParent = transform.Find("Conditions");
            actionsParent = transform.Find("Actions");

            if (eventsParent)
            {
                eventsParent.gameObject.GetComponentsInChildrenDepth1(true, true, trigger.events);
            }
            
            if (conditionsParent)
            {
                conditionsParent.gameObject.GetComponentsInChildrenDepth1(true, true, trigger.conditions);
            }
            
            if (actionsParent)
            {
                actionsParent.gameObject.GetComponentsInChildrenDepth1(true, true, trigger.actions);
            }
        }

        public ITrigger.ExecutionResult Execute()
        {
            return trigger.Execute();
        }
        
        public void ForceQueueExecution()
        {
            trigger.ForceQueueExecution();
        }

        public void ForceQueueExecution(object activator)
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

        public void OnEnable()
        {
            trigger.SetEnabled(true);
        }

        public void OnDisable()
        {
            trigger.SetEnabled(false);
        }

        public bool IsDisabled()
        {
            return !isActiveAndEnabled || trigger.IsDisabled();
        }

        public void ClearPendingExecutions()
        {
            trigger.ClearPendingExecutions();
        }

        public TriggerState GetTriggerState()
        {
            return trigger.GetTriggerState();
        }

        public void LogError(Exception e)
        {
            var triggerState = trigger.GetTriggerState();
            
            if (triggerState.executingAction < trigger.actions.Count)
            {
                var action = trigger.actions[triggerState.executingAction];
                if (action is TriggerAction triggerAction)
                {
                    Debug.LogException(e, triggerAction);
                }
                else
                {
                    Debug.LogException(e, this);
                }
            }
            else
            {
                Debug.LogException(e, this);
            }
        }
    }
}