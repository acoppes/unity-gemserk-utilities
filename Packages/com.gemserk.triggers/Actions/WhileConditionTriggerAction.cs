namespace Gemserk.Triggers.Actions
{
    public class WhileConditionTriggerAction : TriggerAction
    {
        public TriggerCondition condition;
        public TriggerActionGroup actionGroup;

        // private bool running;

        public override string GetObjectName()
        {
            if (condition && actionGroup)
            {
                return $"While({condition.GetObjectName()}, {actionGroup.GetObjectName()})";
            }
            return "While()";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            // if (running)
            // {
            //     var result =actionGroup.Execute(activator);
            //     if (result != ITrigger.ExecutionResult.Completed)
            //     {
            //         return result;
            //     }
            // }
            
            if (condition.Evaluate(activator))
            {
                var result = actionGroup.Execute(activator);
                
                if (result == ITrigger.ExecutionResult.Interrupt)
                {
                    return result;
                }

                // if (result == ITrigger.ExecutionResult.Running)
                // {
                //     running = true;
                //     return result;
                // }
                
                return ITrigger.ExecutionResult.Running;
            }

            return ITrigger.ExecutionResult.Completed;
        }
    }
}