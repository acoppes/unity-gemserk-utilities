namespace Gemserk.Triggers
{
    public interface ITrigger
    {
        public enum ExecutionState
        {
            Waiting,
            PendingExecution,
            Executing
        }
        
        public enum ExecutionResult
        {
            Running,
            Completed
        }
        
        public interface IAction
        {
            ExecutionResult Execute(object activator = null);
        }

        public interface IEvent
        {
            
        }
    
        public interface ICondition
        {
            bool Evaluate(object activator = null);
        }
        
        public ExecutionState State { get; }

        ExecutionResult Execute();

        void QueueExecution(object activator = null);

        void ForceQueueExecution(object activator = null);
        
        void StartExecution();
        
        void CompleteCurrentExecution();
    }
}