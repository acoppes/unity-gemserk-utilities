using Gemserk.Utilities.Signals;

namespace Gemserk.Triggers
{
    [TriggerEditor("Invoke Signal")]
    public class InvokeSignalTriggerAction : TriggerAction
    {
        public SignalAsset signal;

        public override string GetObjectName()
        {
            if (signal != null)
            {
                return $"InvokeSignal({signal.name})";
            }
            return "InvokeSignal()";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            signal.Signal(activator);
            return ITrigger.ExecutionResult.Completed;
        }
    }
}