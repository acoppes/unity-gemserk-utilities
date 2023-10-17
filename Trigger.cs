using System.Collections.Generic;

namespace Gemserk.Triggers
{
    public class Trigger : ITrigger
    {
        public readonly List<ITrigger.IEvent> events = new();
        public readonly List<ITrigger.ICondition> conditions = new();
        public readonly List<ITrigger.IAction> actions = new();

        public int executingAction;
        public int executionTimes;

        public readonly List<object> pendingExecutions = new List<object>();

        private ITrigger.ExecutionState state;
        
        public ITrigger.ExecutionState State => state;

        private object currentActivator => pendingExecutions.Count > 0 ? pendingExecutions[0] : null;
        
        private bool Evaluate(object activator = null)
        {
            var result = true;

            foreach (var condition in conditions)
            {
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
            
            if (state == ITrigger.ExecutionState.Waiting)
            {
                state = ITrigger.ExecutionState.PendingExecution;
            }
        }

        public void QueueExecution(object activator = null)
        {
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
            executingAction = 0;
        }

        public void CompleteCurrentExecution()
        {
            executingAction = 0;
            pendingExecutions.RemoveAt(0);

            executionTimes++;

            if (pendingExecutions.Count == 0)
            {
                state = ITrigger.ExecutionState.Waiting;
            }
            else
            {
                state = ITrigger.ExecutionState.PendingExecution;
            }
        }

        public ITrigger.ExecutionResult Execute()
        {
            while (executingAction < actions.Count)
            {
                var action = actions[executingAction];
                var result = action.Execute(currentActivator);
                
                if (result == ITrigger.ExecutionResult.Running || result == ITrigger.ExecutionResult.Interrupt)
                {
                    return result;
                }
                
                executingAction++;
            }

            executingAction = 0;
            return ITrigger.ExecutionResult.Completed;
        }
    }
}