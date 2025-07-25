using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.Audio;

namespace Game.Components
{
    [CreateAssetMenu(menuName = "Gemserk/Sound Effect")]
    public class SoundEffectAsset : ScriptableObject
    {
        public AudioMixerGroup mixerGroup;
        public List<AudioClip> clips = new List<AudioClip>();
        public float volume = 1;
        public bool loop;
        public MinMaxFloat randomPitch = new MinMaxFloat(1, 1);
        
        public GameObject customSoundEffectPrefab;
    }
}