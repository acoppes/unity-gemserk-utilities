using Gemserk.Triggers;
using UnityEngine;

namespace Game.Triggers
{
    public class AudioSourceTriggerEvent : TriggerEvent
    {
        public enum EventType
        {
            Stop = 0,
            Play = 1
        }

        public EventType eventType = EventType.Stop;
        
        public AudioSource target;

        private bool wasPlaying;

        private void Awake()
        {
            wasPlaying = target.isPlaying;
        }

        public void FixedUpdate()
        {
            if (eventType == EventType.Stop)
            {
                if (wasPlaying && !target.isPlaying)
                {
                    trigger.QueueExecution(target.gameObject);
                }
            }

            if (eventType == EventType.Play)
            {
                if (!wasPlaying && target.isPlaying)
                {
                    trigger.QueueExecution(target.gameObject);
                }
            }

            wasPlaying = target.isPlaying;
        }
    }
}