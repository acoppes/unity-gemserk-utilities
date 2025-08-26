namespace Gemserk.Triggers.Actions
{
    public class BreakIterationTriggerAction : TriggerAction
    {
        public override string GetObjectName()
        {
            return "Break()";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            // just in case we want to handle the break in a custom way.
            var iteration = GetComponentInParent<ITrigger.IActionIteration>();
            iteration?.Break();
            return ITrigger.ExecutionResult.Interrupt;
        }
    }
}