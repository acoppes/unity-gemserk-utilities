using System.Collections;
using Gemserk.Triggers;
using UnityEngine;

namespace Game.Triggers
{
    public class AudioFilterTriggerAction : TriggerAction
    {
        public GameObject target;
        
        public float frequency;
        public float tweenDuration = 0;
        
        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var filter = target.GetComponent<AudioLowPassFilter>();
            // filter.cutoffFrequency = frequency;
            // StartCoroutine(ChangeFrequency(frequency));

            LeanTween.value(this.gameObject, filter.cutoffFrequency, frequency, tweenDuration)
                .setOnUpdate(newFrequency =>
                {
                    filter.cutoffFrequency = newFrequency;
                });
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}