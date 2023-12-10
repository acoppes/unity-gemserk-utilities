using Game.Definitions;
using Gemserk.Leopotam.Ecs;
using MyBox;

namespace Game.Components
{
    public class AnimationComponentDefinition : ComponentDefinitionBase
    {
        public AnimationsAsset animationsAsset;
        public SpritesMetadata spritesMetadata;
        
        public StartingAnimationComponent.StartingAnimationType startingAnimationType = StartingAnimationComponent.StartingAnimationType.Name;
        [ConditionalField(nameof(startingAnimationType), false, StartingAnimationComponent.StartingAnimationType.Name)]
        public string defaultAnimation;
        [ConditionalField(nameof(startingAnimationType), true, StartingAnimationComponent.StartingAnimationType.None)]
        public bool randomizeStartFrame = false;
        [ConditionalField(nameof(startingAnimationType), true, StartingAnimationComponent.StartingAnimationType.None)]
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

            if (startingAnimationType == StartingAnimationComponent.StartingAnimationType.Name &&
                !string.IsNullOrEmpty(defaultAnimation))
            {
                if (!world.HasComponent<StartingAnimationComponent>(entity))
                {
                    world.AddComponent(entity, new StartingAnimationComponent());
                }
                    
                ref var startingAnimationComponent = ref world.GetComponent<StartingAnimationComponent>(entity);
                
                startingAnimationComponent.startingAnimationType = startingAnimationType;
                startingAnimationComponent.randomizeStartFrame = randomizeStartFrame;
                startingAnimationComponent.name = defaultAnimation;
                startingAnimationComponent.loop = defaultAnimationLoop;
            }
                
            if (startingAnimationType == StartingAnimationComponent.StartingAnimationType.Random)
            {
                if (!world.HasComponent<StartingAnimationComponent>(entity))
                {
                    world.AddComponent(entity, new StartingAnimationComponent());
                }
                    
                ref var startingAnimationComponent = ref world.GetComponent<StartingAnimationComponent>(entity);
                
                startingAnimationComponent.startingAnimationType = startingAnimationType;
                startingAnimationComponent.randomizeStartFrame = randomizeStartFrame;
                startingAnimationComponent.loop = defaultAnimationLoop;
            }
        }
    }
}