using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class VisualEffectsSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<VfxComponent, AnimationsComponent>, Exc<DisabledComponent>> animationFilter = default;
        readonly EcsFilterInject<Inc<VfxComponent, AnimationsComponent, DestroyableComponent>, Exc<DisabledComponent>> destroyableFilter = default;
        
        public void Run(EcsSystems systems)
        {
            // play vfx animation if didn't start yet
            foreach (var e in animationFilter.Value)
            {
                ref var vfxComponent = ref animationFilter.Pools.Inc1.Get(e);
                ref var animationComponent = ref animationFilter.Pools.Inc2.Get(e);

                if (!animationComponent.IsPlaying(vfxComponent.animation))
                {
                    animationComponent.Play(vfxComponent.animation, 1);
                }
            }
            
            // destroy if vfx completed
            foreach (var e in destroyableFilter.Value)
            {
                var animationComponent = destroyableFilter.Pools.Inc2.Get(e);
                ref var destroyableComponent = ref destroyableFilter.Pools.Inc3.Get(e);

                if (animationComponent.state == AnimationsComponent.State.Completed)
                {
                    destroyableComponent.destroy = true;
                }
            }
        }
    }
}