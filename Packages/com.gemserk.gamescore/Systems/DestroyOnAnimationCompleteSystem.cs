using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class DestroyOnAnimationCompleteSystem : BaseSystem, IEcsRunSystem
    {
        // Just in case avoid destroying animation entities if they have a pending start animation component.
        readonly EcsFilterInject<Inc<AnimationComponent, DestroyableComponent, DestroyOnAnimationCompleteComponent>, 
            Exc<DisabledComponent, StartingAnimationComponent>> filter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in filter.Value)
            {
                ref var animations = ref filter.Pools.Inc1.Get(entity);

                if (animations.isCompleted)
                {
                    ref var destroyable = ref filter.Pools.Inc2.Get(entity);
                    destroyable.destroy = true;
                }
            }
        }
    }
}