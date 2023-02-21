namespace Gemserk.Triggers
{
    public class WaitForConditionTriggerAction : TriggerAction
    {
        public TriggerCondition condition;
        
        public override string GetObjectName()
        {
            if (condition == null)
            {
                return "WaitForCondition()";
            }
            
            return $"WaitForCondition({condition.GetObjectName()})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            if (!condition.Evaluate(activator))
            {
                return ITrigger.ExecutionResult.Running;
            }

            return ITrigger.ExecutionResult.Completed;
        }
    }
}