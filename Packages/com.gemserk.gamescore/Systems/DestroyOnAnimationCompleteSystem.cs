using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class DestroyOnAnimationCompleteSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<AnimationComponent, DestroyableComponent, DestroyOnAnimationCompleteComponent>, Exc<DisabledComponent>> filter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in filter.Value)
            {
                var animations = filter.Pools.Inc1.Get(entity);
                ref var destroyable = ref filter.Pools.Inc2.Get(entity);

                if (animations.isCompleted)
                {
                    destroyable.destroy = true;
                }
            }
        }
    }
}