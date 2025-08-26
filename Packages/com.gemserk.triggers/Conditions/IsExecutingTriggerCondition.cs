namespace Gemserk.Triggers.Conditions
{
    public class IsExecutingTriggerCondition : TriggerCondition
    {
        private ITrigger trigger;
        
        private void Awake()
        {
            trigger = GetComponentInParent<ITrigger>();
        }

        public override string GetObjectName()
        {
            return "IsExecuting()";
        }
        
        public override bool Evaluate(object activator = null)
        {
            return trigger.State == ITrigger.ExecutionState.Executing;
        }
    }
}