using Gemserk.Triggers;

namespace Game.Triggers.Conditions
{
    public class CompareTriggerCondition : WorldTriggerCondition
    {
        public enum CompareType
        {
            EqualsTo = 0,
            GreaterThan = 1,
            LessThan = 2
        }

        public CompareType compareType;
        
        public ValueProvider valueProviderA;
        public ValueProvider valueProviderB;
        
        public override string GetObjectName()
        {
            return $"{compareType}({valueProviderA}, {valueProviderB})";
        }
        
        public override bool Evaluate(object activator = null)
        {
            var a = valueProviderA.GetIntValue(world, activator);
            var b = valueProviderB.GetIntValue(world, activator);

            return compareType switch
            {
                CompareType.EqualsTo => a == b,
                CompareType.GreaterThan => a > b,
                CompareType.LessThan => a < b,
                _ => false
            };
        }
    }
}