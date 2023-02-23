namespace Gemserk.Triggers
{
    /// <summary>
    /// This trigger action use just used to fill spaces like when performing random actions
    /// </summary>
    public class NoneTriggerAction : TriggerAction
    {
        public override string GetObjectName()
        {
            return "None()";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            return ITrigger.ExecutionResult.Completed;
        }
    }
}