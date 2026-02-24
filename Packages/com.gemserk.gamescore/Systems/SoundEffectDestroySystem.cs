using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class SoundEffectDestroySystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<SoundEffectComponent, DestroyableComponent>> filter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in filter.Value)
            {
                var sfxComponent = filter.Pools.Inc1.Get(entity);
                ref var destroyableComponent = ref filter.Pools.Inc2.Get(entity);

                if (!sfxComponent.autoDestroyOnComplete || !sfxComponent.source)
                    continue;
                
                if (sfxComponent.started && !sfxComponent.source.isPlaying)
                {
                    destroyableComponent.destroy = true;
                }
            }
        }
    }
}