namespace Gemserk.Triggers
{
    public interface ITrigger
    {
        public enum ExecutionState
        {
            Waiting,
            PendingExecution,
            Executing,
            Completed
        }
        
        public enum ExecutionResult
        {
            Running,
            Completed,
            Interrupt
            // TODO: failure result so we could debug and see if it run or not...
        }
        
        public interface IAction
        {
            bool Disabled { get; }
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

        bool IsDisabled();
    }
}