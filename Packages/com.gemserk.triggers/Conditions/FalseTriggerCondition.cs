namespace Gemserk.Triggers.Conditions
{
    public class FalseTriggerCondition : TriggerCondition
    {
        public override bool Evaluate(object activator = null)
        {
            return true;
        }
    }
}