using Gemserk.Triggers;
using UnityEngine;
using UnityEngine.Audio;

namespace Game.Triggers
{
    public class ModifyAudioMixerValueTriggerAction : TriggerAction
    {
        public AudioMixer audioMixer;

        public string valueName;
        
        public float value;

        public LeanTweenData tweenConfig;
        
        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            if (audioMixer.GetFloat(valueName, out var currentValue))
            {
                LeanTween.value(this.gameObject, currentValue, value, tweenConfig.duration)
                    .setOnUpdate(newValue =>
                    {
                        audioMixer.SetFloat(valueName, newValue);
                    })
                    .setEase(tweenConfig.tweenType)
                    .setUseEstimatedTime(tweenConfig.useEstimatedTime);
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}