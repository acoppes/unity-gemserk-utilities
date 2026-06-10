using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public struct CopyAnimationCacheComponent : IEntityComponent
    {
        public int animation;
        public int frame;

        public bool wasModifiedThisFrame;
    }
    
    public class CopyFromAnimationToModelSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler
    {
        readonly EcsFilterInject<Inc<AnimationsComponent, ModelInstanceComponent, CopyAnimationCacheComponent>, Exc<DisabledComponent>> filter = default;
        
        public void OnEntityCreated(World world, Entity entity)
        {
            if (entity.Has<AnimationsComponent>() && entity.Has<ModelComponent>())
            {
                entity.Add(new CopyAnimationCacheComponent()
                {
                    animation = -1,
                    frame = -1
                });
            }
        }
        
        public void Run(EcsSystems systems)
        {
            foreach (var e in filter.Value)
            {
                var animations = filter.Pools.Inc1.Get(e);
                ref var copyAnimationCached = ref filter.Pools.Inc3.Get(e);

                if (animations.currentAnimation == AnimationsComponent.NoAnimation)
                {
                    continue;
                }
                
                copyAnimationCached.wasModifiedThisFrame = false;
                
                ref var modelComponent = ref filter.Pools.Inc2.Get(e);

                if (copyAnimationCached.animation != animations.currentAnimation || 
                    copyAnimationCached.frame != animations.currentFrame)
                {
                    var totalAnimations = animations.animationsAsset.animations.Count;
                    if (animations.currentAnimation < 0 || animations.currentAnimation >= totalAnimations)
                    {
                        Debug.LogError($"CopyFromAnimationToModelSystem: wrong animation index for {e} - {animations.animationsAsset.name} - {animations.currentAnimation} - {animations.lastPlayedAnimationNameForDebug}");
                        continue;
                    }
                    
                    var currentAnimation = animations.animationsAsset.animations[animations.currentAnimation];
                    
                    var totalFrames = currentAnimation.TotalFrames;
                    if (animations.currentFrame < 0 || animations.currentFrame >= totalFrames)
                    {
                        Debug.LogError($"CopyFromAnimationToModelSystem: wrong frame index for {e} - {animations.animationsAsset.name} - {animations.currentAnimation} - {animations.currentFrame} - {animations.lastPlayedAnimationNameForDebug}");
                        continue;
                    }
               
                    var frame = currentAnimation.frames[animations.currentFrame];
                    
                    modelComponent.instance.spriteRenderer.sprite = frame.sprite;

                    copyAnimationCached.animation = animations.currentAnimation;
                    copyAnimationCached.frame = animations.currentFrame;
                    
                    copyAnimationCached.wasModifiedThisFrame = true;
                }
            }
        }
    }
}