using System.Collections.Generic;

namespace Gemserk.Actions
{
    public class Trigger : ITrigger
    {
        public readonly List<ITrigger.IEvent> events = new();
        public readonly List<ITrigger.ICondition> conditions = new();
        public readonly List<ITrigger.IAction> actions = new();

        public int executingAction;

        public int pendingExecutions;

        private ITrigger.ExecutionState state;
        
        public ITrigger.ExecutionState State => state;
        
        public bool Evaluate()
        {
            var result = true;

            foreach (var condition in conditions)
            {
                if (!condition.Evaluate())
                {
                    result = false;
                    break;
                }
            }
            
            return result;
        }

        public void QueueExecution()
        {
            pendingExecutions++;
            
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
            pendingExecutions--;

            if (pendingExecutions <= 0)
            {
                state = ITrigger.ExecutionState.Waiting;
                pendingExecutions = 0;
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
                var result = action.Execute();
                
                if (result == ITrigger.ExecutionResult.Running)
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