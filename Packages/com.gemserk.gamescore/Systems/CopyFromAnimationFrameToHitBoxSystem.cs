using Game.Components;
using Game.Utilities;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class CopyFromAnimationFrameToHitBoxSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<AnimationsComponent, PositionComponent, LookingDirection, HitBoxComponent, AnimationsSpritesMetadata>> filter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var e in filter.Value)
            {
                var animations = filter.Pools.Inc1.Get(e);

                if (animations.currentAnimation == AnimationsComponent.NoAnimation)
                {
                    continue;
                }
                
                var position = filter.Pools.Inc2.Get(e);
                var lookingDirection = filter.Pools.Inc3.Get(e);
                
                ref var hitBox = ref filter.Pools.Inc4.Get(e);
                
                var asset = animations.animationsAsset;
                var animationDefinition = asset.animations[animations.currentAnimation];
                var frame = animationDefinition.frames[animations.currentFrame];

                hitBox.hurt = new HitBox();
                hitBox.hit = new HitBox();

                if (hitBox.defaultHurt)
                {
                    hitBox.hurt = hitBox.defaultHurt.GetHitBox(position, lookingDirection);
                }
                
                // hitBox.hurt.position = new Vector2(position.value.x, position.value.y);
                // hitBox.hurt.offset += new Vector2(0, position.value.z);
                var spritesMetadata = filter.Pools.Inc5.Get(e);
                
                var frameMetadata = spritesMetadata.metadata.GetFrameMetadata(frame.sprite);

                if (frameMetadata != null && frameMetadata.hitBoxes.Count > 0)
                {
                    hitBox.hit = frameMetadata.hitBoxes[0].GetHitBox(position, lookingDirection);
                }
                
                if (frameMetadata != null && frameMetadata.hurtBoxes.Count > 0)
                {
                    hitBox.hurt = frameMetadata.hurtBoxes[0].GetHitBox(position, lookingDirection);
                }
            }
        }
    }
}