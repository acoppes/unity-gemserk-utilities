using System.Collections.Generic;

namespace Gemserk.Triggers
{
    public class TriggerState
    {
        public int executionTimes;
        public int maxExecutionTimes;
        public int executingAction;
    }
    
    public class Trigger : ITrigger
    {
        public string name;
        
        public readonly List<ITrigger.IEvent> events = new();
        public readonly List<ITrigger.ICondition> conditions = new();
        public readonly List<ITrigger.IAction> actions = new();
        
        private readonly TriggerState triggerState = new TriggerState();

        public readonly List<object> pendingExecutions = new List<object>();

        private ITrigger.ExecutionState state;

        public string Name => name;
        
        public ITrigger.ExecutionState State => state;

        private object currentActivator => pendingExecutions.Count > 0 ? pendingExecutions[0] : null;

        private bool isDisabled;

        public void SetEnabled(bool enabled)
        { 
            isDisabled = !enabled;

            if (isDisabled)
            {
                ClearPendingExecutions();
            }
        }
        
        private bool Evaluate(object activator = null)
        {
            var result = true;

            foreach (var condition in conditions)
            {
                if (condition.Disabled)
                    continue;
                
                if (!condition.Evaluate(activator))
                {
                    result = false;
                    break;
                }
            }
            
            return result;
        }

        public void ForceQueueExecution(object activator = null)
        {
            pendingExecutions.Add(activator);
            
            if (state == ITrigger.ExecutionState.Waiting || state == ITrigger.ExecutionState.Completed)
            {
                state = ITrigger.ExecutionState.PendingExecution;
            }
        }

        public void QueueExecution(object activator = null)
        {
            if (isDisabled)
            {
                return;
            }
            
            if (state == ITrigger.ExecutionState.Completed)
            {
                return;
            }
            
            if (!Evaluate(activator))
            {
                return;
            }
            
            pendingExecutions.Add(activator);
            
            if (state == ITrigger.ExecutionState.Waiting)
            {
                state = ITrigger.ExecutionState.PendingExecution;
            }
        }

        public void StartExecution()
        {
            state = ITrigger.ExecutionState.Executing;
            triggerState.executingAction = 0;
        }

        public void CompleteCurrentExecution()
        {
            triggerState.executingAction = 0;
            
            if (pendingExecutions.Count > 0)
            {
                pendingExecutions.RemoveAt(0);
            }
            
            triggerState.executionTimes++;

            if (pendingExecutions.Count == 0)
            {
                state = ITrigger.ExecutionState.Waiting;
            }
            else
            {
                state = ITrigger.ExecutionState.PendingExecution;
            }

            if (triggerState.maxExecutionTimes > 0 && triggerState.executionTimes >= triggerState.maxExecutionTimes)
            {
                state = ITrigger.ExecutionState.Completed;
            }
        }

        public bool IsDisabled()
        {
            return isDisabled;
        }

        public void ClearPendingExecutions()
        {
            pendingExecutions.Clear();
        }

        public TriggerState GetTriggerState()
        {
            return triggerState;
        }

        public ITrigger.ExecutionResult Execute()
        {
            while (triggerState.executingAction < actions.Count)
            {
                var action = actions[triggerState.executingAction];

                var result = ITrigger.ExecutionResult.Completed;
                
                // will assume disabled actions are ignored for now.
                if (!action.Disabled)
                {
                    result = action.Execute(currentActivator);
                }
                
                if (result == ITrigger.ExecutionResult.Running || result == ITrigger.ExecutionResult.Interrupt)
                {
                    return result;
                }
                
                triggerState.executingAction++;
            }

            triggerState.executingAction = 0;
            return ITrigger.ExecutionResult.Completed;
        }
    }
}