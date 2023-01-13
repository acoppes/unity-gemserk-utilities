using System.Collections.Generic;

namespace Gemserk.Actions
{
    public class Trigger : ITrigger
    {
        public readonly List<ITrigger.IEvent> events = new List<ITrigger.IEvent>();
        public readonly List<ITrigger.ICondition> conditions = new List<ITrigger.ICondition>();
        public readonly List<ITrigger.IAction> actions = new List<ITrigger.IAction>();

        private int executingAction;

        // private int pendingExecutions;

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
        
        public void StartExecution()
        {
            state = ITrigger.ExecutionState.Executing;
            executingAction = 0;
        }

        public void StopExecution()
        {
            state = ITrigger.ExecutionState.Waiting;
            executingAction = 0;
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