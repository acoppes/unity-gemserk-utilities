using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using MyBox;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.Components
{
    public struct SoundEffectComponent : IEntityComponent
    {
        public SoundEffectAsset soundEffect;
        
        public List<AudioClip> clips => soundEffect.clips;
        
        public GameObject prefab;
        
        public bool autoDestroyOnComplete;
        public AudioSource source;
        public bool started;

        public float volume => soundEffect.volume * volumeMultiplier;
        public float volumeMultiplier;
        
        public bool loop => soundEffect.loop;

        public MinMaxFloat randomStartPitch => soundEffect.randomPitch;
        
        public float pitch;
    }
    
    public class SoundEffectComponentDefinition : ComponentDefinitionBase
    {
        public SoundEffectAsset soundEffect;
        
        public List<AudioClip> clips = new List<AudioClip>();
        
        public GameObject prefab;
        public bool autoDestroyOnComplete;

        public float volume = 1;
        public bool loop;

        public MinMaxFloat randomPitch = new MinMaxFloat(1, 1);

        public override void Apply(World world, Entity entity)
        {
            Assert.IsFalse(clips.Count > 0 && prefab, "Should use clips or prefab, not both");

            world.AddComponent(entity, new SoundEffectComponent
            {
                soundEffect = soundEffect,
                prefab = prefab,
                autoDestroyOnComplete = autoDestroyOnComplete,
                volumeMultiplier = 1f,
                pitch = 1
            });
        }
    }
}