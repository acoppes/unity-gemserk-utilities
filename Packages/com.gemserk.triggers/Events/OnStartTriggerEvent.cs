namespace Gemserk.Triggers.Events
{
    public class OnStartTriggerEvent : TriggerEvent
    {
        public enum ActionType
        {
            OnStart = 0,
            OnAwake = 1
        }

        public ActionType actionType = ActionType.OnStart;
        
        public override string GetObjectName()
        {
            if (actionType == ActionType.OnStart)
                return "OnStart";
            return "OnAwake";
        }

        private void Awake()
        {
            if (actionType == ActionType.OnAwake)
            {
                trigger.QueueExecution();
            }
        }
        
        private void Start()
        {
            if (actionType == ActionType.OnStart)
            {
                trigger.QueueExecution();
            }
        }
    }
}