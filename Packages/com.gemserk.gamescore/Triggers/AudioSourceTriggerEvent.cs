using Gemserk.Triggers;
using UnityEngine;

namespace Game.Triggers
{
    public class AudioSourceTriggerEvent : TriggerEvent
    {
        public enum EventType
        {
            Stop = 0,
            Play = 1,
            Completed = 2
        }

        public EventType eventType = EventType.Stop;
        
        public AudioSource target;

        private bool wasPlaying;
        private float lastTime;

        private AudioClip previousClip;

        private void Awake()
        {
            wasPlaying = target.isPlaying;
            previousClip = target.clip;
            lastTime = -1f;
        }

        public void Update()
        {
            if (previousClip != target.clip)
            {
                lastTime = -1f;
                wasPlaying = target.isPlaying;
                previousClip = target.clip;
            }

            if (!target || !target.clip)
            {
                return;
            }
            
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
            
            if (eventType == EventType.Completed)
            {
                // I assume audio source goes to 0 on completed.
                if (lastTime > target.time)
                {
                    trigger.QueueExecution(target.gameObject);
                }
            }

            wasPlaying = target.isPlaying;
            lastTime = target.time;
        }
    }
}