using Gemserk.Triggers;
using Gemserk.Utilities.Signals;

namespace Gemserk.Actions
{
    public class OnSignalTriggerEvent : TriggerEvent
    {
        public SignalAsset signal;
        
        public override string GetObjectName()
        {
            if (signal != null)
            {
                return $"OnSignal({signal.name})";
            }
            return "OnSignal()";
        }

        private void OnEnable()
        {
            if (signal != null)
            {
                signal.Register(OnSignal);
            }
        }

        private void OnDisable()
        {
            if (signal != null)
            {
                signal.Unregister(OnSignal);
            }
        }

        private void OnSignal(object userdata)
        {
            trigger.QueueExecution(userdata);
        }
    }
}
