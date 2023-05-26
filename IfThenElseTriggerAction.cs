namespace Gemserk.Triggers
{
    public class IfThenElseTriggerAction : TriggerAction
    {
        public TriggerCondition condition;
        public TriggerActionGroup thenGroup;
        public TriggerActionGroup elseGroup;

        private TriggerActionGroup runningGroup;

        public override string GetObjectName()
        {
            if (thenGroup == null && elseGroup == null)
            {
                return "If()";
            }
            
            if (thenGroup != null && elseGroup == null)
            {
                return $"IfThen({thenGroup.name})";
            }

            return $"IfThenElse({thenGroup.name}, {elseGroup.name})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            ITrigger.ExecutionResult result;
            
            // if we already have a running
            if (runningGroup != null)
            {
                result = runningGroup.Execute(activator);
                if (result == ITrigger.ExecutionResult.Completed)
                {
                    runningGroup = null;
                }
                return result;
            }

            if (condition.Evaluate(activator))
            {
                runningGroup = thenGroup;
            }
            else
            {
                runningGroup = elseGroup;
            }

            if (runningGroup == null)
            {
                return ITrigger.ExecutionResult.Completed;
            }
            
            result = runningGroup.Execute(activator);
            if (result == ITrigger.ExecutionResult.Completed)
            {
                runningGroup = null;
            }
            return result;
        }
    }
}