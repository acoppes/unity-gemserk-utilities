using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class AnimationFromHealthSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<AnimationsComponent, AnimationFromHealthComponent, HealthComponent>, Exc<DisabledComponent>> 
            animationsFromResistance = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var e in animationsFromResistance.Value)
            {
                ref var animations = ref animationsFromResistance.Pools.Inc1.Get(e);
                ref var health = ref animationsFromResistance.Pools.Inc3.Get(e);

                var currentAnimation = animations.animationsAsset.animations[animations.currentAnimation];
                
                var currentFrameFromResistance = Mathf.RoundToInt((currentAnimation.TotalFrames - 1) * health.factor);
                animations.currentFrame = Mathf.Clamp(currentFrameFromResistance, 0, currentAnimation.TotalFrames - 1);
            }
        }
    }
}