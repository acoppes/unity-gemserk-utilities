using Gemserk.Triggers;
using UnityEngine;
using UnityEngine.Audio;

namespace Game.Triggers
{
    public class ModifyAudioMixerValueTriggerAction : TriggerAction
    {
        public AudioMixer audioMixer;

        public string valueName;
        
        // public float value;

        public LeanTweenConfiguration tween;

        public bool useCurrentValue;

        public override string GetObjectName()
        {
            return $"TweenAudioMixerValue({valueName}, {tween.from.x}, {tween.to.x}, time:{tween.time})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            if (audioMixer.GetFloat(valueName, out var currentValue))
            {
                var from = useCurrentValue ? currentValue : tween.from.x;
                
                LeanTween.value(gameObject, from, tween.to.x, tween.time)
                    .setOnUpdate(newValue =>
                    {
                        audioMixer.SetFloat(valueName, newValue);
                    })
                    .setEase(tween.easing)
                    .setUseEstimatedTime(tween.useEstimatedTime);
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}