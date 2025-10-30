namespace Gemserk.Triggers.Actions
{
    public class WaitForTriggerExecutionTriggerAction : TriggerAction
    {
        public TriggerObject trigger;
        
        public override string GetObjectName()
        {
            if (!trigger)
            {
                return "WaitForTriggerExecution()";
            }
            
            return $"WaitForTriggerExecution({trigger.name})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            if (trigger.State == ITrigger.ExecutionState.Executing ||
                trigger.State == ITrigger.ExecutionState.PendingExecution)
            {
                return ITrigger.ExecutionResult.Running;
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}