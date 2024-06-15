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

        // IEnumerator ChangeFrequency(float targetFrequency)
        // {
        //     var filter = target.GetComponent<AudioLowPassFilter>();
        //     var d = targetFrequency - filter.cutoffFrequency;
        //     
        //     if (tweenDuration < Mathf.Epsilon)
        //     {
        //         filter.cutoffFrequency = targetFrequency;
        //         yield break;
        //     }
        //     
        //     var deltaFrequency = d / tweenDuration;
        //     while (Mathf.Abs(targetFrequency - filter.cutoffFrequency) > 1)
        //     {
        //         filter.cutoffFrequency += deltaFrequency * Time.deltaTime;
        //         yield return null;
        //     }
        //     
        // }
    }
}