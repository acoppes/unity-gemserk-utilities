using Gemserk.Utilities;
using UnityEngine;

namespace Gemserk.Triggers
{
    public class TimeTriggerEvent : TriggerEvent
    {
        public Cooldown time;
        public float current;

        public override string GetObjectName()
        {
            return $"Time({time.Total})";
        }

        private void Start()
        {
            time.Increase(current);
        }

        private void FixedUpdate()
        {
            time.Increase(Time.deltaTime);
            if (time.IsReady)
            {
                trigger.QueueExecution(null);
                time.Reset();
            }
        }
    }
    
}