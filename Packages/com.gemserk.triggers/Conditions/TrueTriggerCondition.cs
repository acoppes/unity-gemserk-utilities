namespace Gemserk.Triggers.Conditions
{
    public class TrueTriggerCondition : TriggerCondition
    {
        public override bool Evaluate(object activator = null)
        {
            return true;
        }
    }
}