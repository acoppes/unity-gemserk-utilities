using Gemserk.Leopotam.Ecs;
using MyBox;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.Components
{
    public struct SoundEffectComponent : IEntityComponent
    {
        public AudioClip clip;
        public GameObject prefab;
        
        public bool autoDestroyOnComplete;
        public AudioSource source;
        public bool started;

        public float volume;
        public bool loop;

        public MinMaxFloat randomStartPitch;
        public float pitch;
    }
    
    public class SoundEffectComponentDefinition : ComponentDefinitionBase
    {
        public AudioClip clip;
        public GameObject prefab;
        public bool autoDestroyOnComplete;

        public float volume = 1;
        public bool loop;

        public MinMaxFloat randomPitch = new MinMaxFloat(1, 1);

        public override void Apply(World world, Entity entity)
        {
            Assert.IsFalse(clip != null && prefab != null, "Should set clip or prefab, not both");
            
            world.AddComponent(entity, new SoundEffectComponent
            {
                clip = clip,
                prefab = prefab,
                autoDestroyOnComplete = autoDestroyOnComplete,
                volume = volume,
                loop = loop,
                randomStartPitch = randomPitch,
                pitch = 1
            });
        }
    }
}