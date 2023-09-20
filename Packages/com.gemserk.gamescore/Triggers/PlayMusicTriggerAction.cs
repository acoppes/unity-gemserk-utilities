using Gemserk.Triggers;
using UnityEngine;

namespace Game.Triggers
{
    public class PlayMusicTriggerAction : WorldTriggerAction
    {
        public enum ActionType
        {
            Play = 0,
            Stop = 1
        }
        
        [SerializeField]
        private AudioSource musicSource;

        public ActionType actionType;
        public bool loop;

        public override string GetObjectName()
        {
            if (musicSource != null)
            {
                return $"Music{actionType}({musicSource.name}, {loop})";
            }
            return $"Music{actionType}({loop})";
        }
        
        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            if (actionType == ActionType.Play)
            {
                musicSource.loop = loop;
                musicSource.Play();
            }
            
            if (actionType == ActionType.Stop)
            {
                musicSource.Stop();
            }  

            return ITrigger.ExecutionResult.Completed;
        }
    }
}