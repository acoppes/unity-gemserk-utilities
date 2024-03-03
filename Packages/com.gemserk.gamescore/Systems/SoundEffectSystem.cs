using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities.Pooling;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MyBox;
using UnityEngine;

namespace Game.Systems
{
    public class SoundEffectSystem : BaseSystem, IEcsInitSystem, IEcsRunSystem, IEntityCreatedHandler, IEntityDestroyedHandler
    {
        [SerializeField]
        private GameObject sfxDefaultPrefab;

        readonly EcsFilterInject<Inc<SoundEffectComponent>, Exc<DisabledComponent>> filter = default;
        
        private GameObjectPoolMap poolMap;
        
        public void Init(EcsSystems systems)
        {
            poolMap = new GameObjectPoolMap("~SfxPool");
        }

        public void OnEntityCreated(World world, Entity entity)
        {
            if (entity.Has<SoundEffectComponent>())
            {
                ref var sfxComponent = ref entity.Get<SoundEffectComponent>();

                if (sfxComponent.clip != null)
                {
                    var prefab = sfxDefaultPrefab;
                    var audioInstance = poolMap.Get(prefab);
                    sfxComponent.source = audioInstance.GetComponent<AudioSource>();
                    sfxComponent.source.clip = sfxComponent.clip;
                } else if (sfxComponent.prefab != null)
                {
                    var prefab = sfxComponent.prefab;
                    var audioInstance = poolMap.Get(prefab);
                    sfxComponent.source = audioInstance.GetComponent<AudioSource>();
                }
            }
        }

        public void OnEntityDestroyed(World world, Entity entity)
        {
            if (entity.Has<SoundEffectComponent>())
            {
                ref var sfxComponent = ref entity.Get<SoundEffectComponent>();
                
                if (sfxComponent.source != null)
                {
                    if (sfxComponent.clip != null)
                    {
                        sfxComponent.source.Stop();
                        sfxComponent.source.clip = null;

                        poolMap.Release(sfxComponent.source.gameObject);
                        sfxComponent.source = null;

                    }
                    else if (sfxComponent.prefab != null)
                    {
                        sfxComponent.source.Stop();
                        poolMap.Release(sfxComponent.source.gameObject);
                        sfxComponent.source = null;
                    }
                }
            }
        }
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in filter.Value)
            {
                ref var sfxComponent = ref filter.Pools.Inc1.Get(entity);
                
                if (!sfxComponent.started)
                {
                    sfxComponent.source.loop = sfxComponent.loop;
                    sfxComponent.source.pitch = sfxComponent.randomPitch.RandomInRange();
                    
                    sfxComponent.source.Play();
                    sfxComponent.started = true;
                }
                
                sfxComponent.source.volume = sfxComponent.volume;
            }
        }

    }
}