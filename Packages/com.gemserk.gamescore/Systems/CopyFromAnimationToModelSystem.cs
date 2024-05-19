using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public struct CopyAnimationCacheComponent : IEntityComponent
    {
        public int animation;
        public int frame;
    }
    
    public class CopyFromAnimationToModelSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler
    {
        readonly EcsFilterInject<Inc<AnimationsComponent, ModelComponent, CopyAnimationCacheComponent>, Exc<DisabledComponent>> filter = default;
        
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
                
                ref var modelComponent = ref filter.Pools.Inc2.Get(e);

                if (copyAnimationCached.animation != animations.currentAnimation || 
                    copyAnimationCached.frame != animations.currentFrame)
                {
                    var currentAnimationDefinition = animations.animationsAsset.animations[animations.currentAnimation];
                    var frame = currentAnimationDefinition.frames[animations.currentFrame];
                    
                    modelComponent.instance.spriteRenderer.sprite = frame.sprite;

                    copyAnimationCached.animation = animations.currentAnimation;
                    copyAnimationCached.frame = animations.currentFrame;
                }
            }
        }
    }
}