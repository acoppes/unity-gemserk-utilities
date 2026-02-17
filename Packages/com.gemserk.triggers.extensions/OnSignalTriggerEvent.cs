using Gemserk.Utilities.Signals;

namespace Gemserk.Triggers
{
    public class OnSignalTriggerEvent : TriggerEvent
    {
        public SignalAsset signal;
        
        public override string GetObjectName()
        {
            if (signal)
            {
                return $"OnSignal({signal.name})";
            }
            return "OnSignal()";
        }

        private void OnEnable()
        {
            if (signal)
            {
                signal.Register(OnSignal);
            }
        }

        private void OnDisable()
        {
            if (signal)
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
