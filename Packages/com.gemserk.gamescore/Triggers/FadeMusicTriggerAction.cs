using Gemserk.Triggers;
using UnityEngine;

namespace Game.Triggers
{
    public class FadeMusicTriggerAction : WorldTriggerAction
    {
        [SerializeField]
        private AudioSource musicSource;

        public float from, to;
        public float time;

        public override string GetObjectName()
        {
            if (musicSource != null)
            {
                return $"MusicFade({musicSource.name}, {from}, {to}, {time:0.0})";
            }
            return $"MusicFade({from}, {to}, {time:0.0})";
        }
        
        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            LeanTweenExtensions.fadeAudio(musicSource, from, to, time);
            return ITrigger.ExecutionResult.Completed;
        }
    }
}