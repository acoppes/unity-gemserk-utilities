using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace Game.Systems
{
    public class CurrentAnimationAttackSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var animations = world.GetComponents<AnimationsComponent>();
            var currentFrames = world.GetComponents<CurrentAnimationAttackComponent>();
            var hitBoxes = world.GetComponents<HitBoxComponent>();
            
            foreach (var entity in world.GetFilter<AnimationsComponent>()
                         .Inc<CurrentAnimationAttackComponent>().Inc<HitBoxComponent>().End())
            {
                var animationComponent = animations.Get(entity);
                var hitBox = hitBoxes.Get(entity);
                ref var currentAnimationFrameComponent = ref currentFrames.Get(entity);
                
                currentAnimationFrameComponent.currentFrameHit = false;

                if (animationComponent.currentAnimation == AnimationsComponent.NoAnimation)
                {
                    continue;
                }
                
                // only updates hit on frame change
                if (animationComponent.currentAnimation != currentAnimationFrameComponent.animation || animationComponent.currentFrame != currentAnimationFrameComponent.frame)
                {
                    currentAnimationFrameComponent.currentFrameHit = hitBox.hit.size.sqrMagnitude > Mathf.Epsilon;
                    currentAnimationFrameComponent.animation = animationComponent.currentAnimation;
                    currentAnimationFrameComponent.frame = animationComponent.currentFrame;
                }
            }
        }
    }
}