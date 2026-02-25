using Gemserk.Triggers;
using MyBox;
using UnityEngine;

namespace Game.Triggers
{
    public class PlayAudioTriggerAction : WorldTriggerAction
    {
        public enum ActionType
        {
            Play = 0,
            Stop = 1, 
            Seek = 2,
            SeekAlpha = 3
        }
        
        public AudioSource musicSource;

        public ActionType actionType;
        [ConditionalField(nameof(actionType), false, ActionType.Play)]
        public bool loop;

        [ConditionalField(nameof(actionType), false, ActionType.Play, ActionType.Seek, ActionType.SeekAlpha)]
        public float time;

        public override string GetObjectName()
        {
            if (musicSource != null)
            {
                if (actionType == ActionType.Play)
                {
                    return $"Audio{actionType}({musicSource.name}, {loop}, {time}s)";
                }
                
                if (actionType == ActionType.Seek || actionType == ActionType.SeekAlpha)
                {
                    return $"Audio{actionType}({musicSource.name}, {time}s)";
                }
                
                return $"Audio{actionType}({musicSource.name})";
            }
            return $"Audio{actionType}({loop})";
        }

        private float GetSeekTime()
        {
            var duration = musicSource.clip.length;
            var seekTime = time;

            if (seekTime > duration)
            {
                seekTime = duration;
            }

            if (time < 0)
            {
                seekTime = duration + time;
            }

            return seekTime;
        }
        
        
        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            if (actionType == ActionType.Play)
            {
                musicSource.loop = loop;
                musicSource.time = GetSeekTime();
                musicSource.Play();
            }
            
            if (actionType == ActionType.Stop)
            {
                musicSource.Stop();
            }  
            
            if (actionType == ActionType.Seek)
            {
                musicSource.time = GetSeekTime();
            }  
            
            if (actionType == ActionType.SeekAlpha)
            {
                var duration = musicSource.clip.length;
                musicSource.time = Mathf.Lerp(0, duration, time);
            }  

            return ITrigger.ExecutionResult.Completed;
        }
    }
}