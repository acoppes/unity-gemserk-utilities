using Game.Components;
using Game.Utilities;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;

namespace Game.Systems
{
    public class CopyFromAnimationFrameToHitBoxSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var animations = world.GetComponents<AnimationsComponent>();
            var hitBoxes = world.GetComponents<HitBoxComponent>();
            var positions = world.GetComponents<PositionComponent>();
            var lookingDirections = world.GetComponents<LookingDirection>();

            foreach (var entity in world.GetFilter<AnimationsComponent>()
                         .Inc<HitBoxComponent>()
                         .Inc<PositionComponent>()
                         .Inc<LookingDirection>().End())
            {
                var animationComponent = animations.Get(entity);

                if (animationComponent.currentAnimation == AnimationsComponent.NoAnimation)
                {
                    continue;
                }
                
                var position = positions.Get(entity);
                var lookingDirection = lookingDirections.Get(entity);
                
                ref var hitBox = ref hitBoxes.Get(entity);
                
                var asset = animationComponent.animationsAsset;
                var animationDefinition = asset.animations[animationComponent.currentAnimation];
                var frame = animationDefinition.frames[animationComponent.currentFrame];

                hitBox.hurt = new HitBox();
                hitBox.hit = new HitBox();

                if (hitBox.defaultHurt != null)
                {
                    hitBox.hurt = hitBox.defaultHurt.GetHitBox(position, lookingDirection);
                }
                
                // hitBox.hurt.position = new Vector2(position.value.x, position.value.y);
                // hitBox.hurt.offset += new Vector2(0, position.value.z);

                var frameMetadata = animationComponent.metadata.GetFrameMetadata(frame.sprite);

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