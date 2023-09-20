using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class CopyFromAnimationToModelSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<AnimationComponent, ModelComponent>, Exc<DisabledComponent>> filter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in filter.Value)
            {
                var animationComponent = filter.Pools.Inc1.Get(entity);
                ref var modelComponent = ref filter.Pools.Inc2.Get(entity);

                if (animationComponent.currentAnimation == AnimationComponent.NoAnimation)
                {
                    continue;
                }
                
                var animation = animationComponent.animationsAsset.animations[animationComponent.currentAnimation];
                var frame = animation.frames[animationComponent.currentFrame];
                
                if (modelComponent.instance.spriteRenderer != null)
                {
                    modelComponent.instance.spriteRenderer.sprite = frame.sprite;
                }
            }
        }
    }
}