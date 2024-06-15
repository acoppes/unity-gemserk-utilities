using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class LeanTweenConfiguration
    {
        public Vector3 from;
        public Vector3 to;
        public float time;
        public LeanTweenType easing = LeanTweenType.linear;
        public bool useEstimatedTime = false;
    }

    [Serializable]
    public class LeanTweenData
    {
        public float duration;
        public LeanTweenType tweenType = LeanTweenType.linear;
        public bool useEstimatedTime = false;
    }
    
    public static class LeanTweenExtensions
    {
        // ReSharper disable once InconsistentNaming
        public static LTDescr fadeAudio(AudioSource source, float from, float to, float time)
        {
            return LeanTween.value(source.gameObject, delegate(float volume, object music)
            {
                var audioSource = music as AudioSource;
                audioSource.volume = volume;
            }, from, to, time).setOnUpdateParam(source);
        }     
        
        // ReSharper disable once InconsistentNaming
        public static LTDescr fadeAudio(AudioSource source, float to, float time)
        {
            return fadeAudio(source, source.volume, to, time);
        }     
    }
}