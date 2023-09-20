using Game.Definitions;
using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public class AnimationComponentDefinition : ComponentDefinitionBase
    {
        public AnimationsAsset animationsAsset;
        public SpritesMetadata spritesMetadata;
        
        public string defaultAnimation;
        public bool defaultAnimationLoop = true;
        
        // TODO: int loops to be configured here optional
        
        public override string GetComponentName()
        {
            return nameof(AnimationComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new AnimationComponent
            {
                animationsAsset = animationsAsset,
                metadata = spritesMetadata,
                currentAnimation = AnimationComponent.NoAnimation,
                currentFrame = AnimationComponent.NoFrame,
                currentTime = 0,
                state = AnimationComponent.State.Completed,
                loops = 0,
                paused = false,
                speed = 1
            });

            if (!string.IsNullOrEmpty(defaultAnimation))
            {
                if (!world.HasComponent<StartingAnimationComponent>(entity))
                {
                    world.AddComponent(entity, new StartingAnimationComponent());
                }

                ref var startingAnimationComponent = ref world.GetComponent<StartingAnimationComponent>(entity);
                
                startingAnimationComponent.loop = defaultAnimationLoop;
                startingAnimationComponent.name = defaultAnimation;
                startingAnimationComponent.randomizeStartFrame = false;
                startingAnimationComponent.startingAnimationType = StartingAnimationComponent.StartingAnimationType.Name;
            }
        }
    }
}