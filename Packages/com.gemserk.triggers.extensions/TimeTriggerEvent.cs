using Gemserk.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gemserk.Triggers
{
    public class TimeTriggerEvent : TriggerEvent
    {
        public Cooldown time;
        
        [FormerlySerializedAs("current")] 
        [Tooltip("Just the starting value of the current")]
        // starting value
        public float startingValue;

        public override string GetObjectName()
        {
            return $"Time({time.Total})";
        }

        private void Start()
        {
            time.Increase(startingValue);
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