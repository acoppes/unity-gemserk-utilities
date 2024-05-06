using Gemserk.Triggers;

namespace Cfinger.KAS.Triggers.Events
{
    public class OnStartTriggerEvent : TriggerEvent
    {
        public override string GetObjectName()
        {
            return "OnStart";
        }

        private void Start()
        {
            trigger.QueueExecution();
        }
    }
}