namespace Gemserk.Actions
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
            ExecutionResult Execute();
        }

        public interface IEvent
        {
            
        }
    
        public interface ICondition
        {
            bool Evaluate();
        }
        
        public ExecutionState State { get; }

        bool Evaluate();
        
        ExecutionResult Execute();

        void QueueExecution();
        
        void StartExecution();
        
        void CompleteCurrentExecution();
    }
}