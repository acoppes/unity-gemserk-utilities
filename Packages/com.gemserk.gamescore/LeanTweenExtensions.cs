using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public struct LeanTweenConfiguration
    {
        public Vector3 from;
        public Vector3 to;
        public float time;
        public LeanTweenType easing;
        public bool useEstimatedTime;

        public bool useCurrentValueForFrom;

        public int loopCount;
        public bool pingPong;
        
        public static LeanTweenConfiguration Default()
        {
            return new LeanTweenConfiguration()
            {
                easing = LeanTweenType.linear,
                useEstimatedTime = false
            };
        }
    }

    public static class LeanTweenExtensions
    {
        // ReSharper disable once InconsistentNaming
        public static LTDescr fadeAudio(AudioSource source, float from, float to, float time)
        {
            return LeanTween.value(source.gameObject, delegate(float volume, object music)
            {
                source.volume = volume;
                // if (music is AudioSource audioSource)
                // {
                //     audioSource.volume = volume;
                // } else if (music is GameObject go)
                // {
                //     var gameObjectAudioSource = go.GetComponent<AudioSource>();
                //     gameObjectAudioSource.volume = volume;
                // }
            }, from, to, time);
            
            // .setOnUpdateParam(source);
            // .setOnCompleteParam(source)
            // .setOnComplete(music =>
            // {
            //     var audioSource = music as AudioSource;
            //     audioSource.volume = to;
            // });
        }     
        
        // ReSharper disable once InconsistentNaming
        public static LTDescr fadeAudio(AudioSource source, float to, float time)
        {
            return fadeAudio(source, source.volume, to, time);
        }

        public static LTDescr scale(this LeanTweenConfiguration tweenConfig, GameObject gameObject)
        {
            var ltDescr = LeanTween.scale(gameObject, tweenConfig.to, tweenConfig.time)
                .setEase(tweenConfig.easing)
                .setUseEstimatedTime(tweenConfig.useEstimatedTime);
            if (!tweenConfig.useCurrentValueForFrom)
            {
                ltDescr.setFrom(tweenConfig.from);
            }
            return ltDescr;
        }
    }
}