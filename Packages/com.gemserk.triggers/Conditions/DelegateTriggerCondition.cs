namespace Gemserk.Triggers.Conditions
{
    public class DelegateTriggerCondition : TriggerCondition
    {
        public TriggerCondition delegateCondition;
        
        public override string GetObjectName()
        {
            if (delegateCondition)
            {
                return $"->({delegateCondition.GetObjectName()})";
            }
            return "->()";
        }
        
        public override bool Evaluate(object activator = null)
        {
            return delegateCondition.Evaluate(activator);
        }
    }
}