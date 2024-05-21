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
        readonly EcsFilterInject<Inc<SoundEffectComponent>, Exc<DisabledComponent>> filter = default;
        readonly EcsFilterInject<Inc<PositionComponent, SoundEffectComponent>, Exc<DisabledComponent>> positionFilter = default;
        
        [SerializeField]
        private GameObject sfxDefaultPrefab;
        private GameObjectPoolMap poolMap;

        public float spatialBlendMinDistance = 0;
        public float spatialBlendMaxDistance = 3;

        private AudioListener listener;
        
        public void Init(EcsSystems systems)
        {
            poolMap = new GameObjectPoolMap("~SfxPool");
            listener = FindFirstObjectByType<AudioListener>();
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
            foreach (var e in filter.Value)
            {
                ref var sfxComponent = ref filter.Pools.Inc1.Get(e);
                
                if (!sfxComponent.started)
                {
                    sfxComponent.source.loop = sfxComponent.loop;
                    sfxComponent.source.pitch = sfxComponent.randomPitch.RandomInRange();
                    
                    sfxComponent.source.Play();
                    sfxComponent.started = true;
                }
                
                sfxComponent.source.volume = sfxComponent.volume;
            }

            if (listener != null)
            {
                var sqrMinDistance = spatialBlendMinDistance * spatialBlendMinDistance;
                var sqrMaxDistance = spatialBlendMaxDistance * spatialBlendMaxDistance;
                
                foreach (var e in positionFilter.Value)
                {
                    // I don't care about 3d objects for now
                    ref var position = ref positionFilter.Pools.Inc1.Get(e);
                    ref var sfx = ref positionFilter.Pools.Inc2.Get(e);

                    var d = (position.value - listener.transform.position).sqrMagnitude;

                    var t = (d * d - sqrMinDistance) / (sqrMaxDistance - sqrMinDistance);
                    
                    sfx.source.spatialBlend = Mathf.Lerp(0f, 1f, t);
                    sfx.source.transform.position = position.value;
                }
            }
            else
            {
                foreach (var e in positionFilter.Value)
                {
                    // I don't care about 3d objects for now
                    ref var position = ref positionFilter.Pools.Inc1.Get(e);
                    ref var sfx = ref positionFilter.Pools.Inc2.Get(e);
                    sfx.source.spatialBlend = 1;
                    sfx.source.transform.position = position.value;
                }
            }
           
        }

    }
}