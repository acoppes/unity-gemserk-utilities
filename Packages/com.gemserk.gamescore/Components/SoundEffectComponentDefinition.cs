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
        
        public bool autoDestroyOnComplete;

        public override void Apply(World world, Entity entity)
        {
            Assert.IsTrue(soundEffect, "Need a sound effect asset to work.");

            world.AddComponent(entity, new SoundEffectComponent
            {
                soundEffect = soundEffect,
                // prefab = prefab,
                autoDestroyOnComplete = autoDestroyOnComplete,
                volumeMultiplier = 1f,
                pitch = 1
            });
        }
    }
}