using System;
using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class DirectionalAnimationSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler, IEntityDestroyedHandler
    {
        readonly EcsFilterInject<Inc<AnimationDirectionsComponent, StartingAnimationComponent>, Exc<DisabledComponent>> startingAnimationFilter = default;
        readonly EcsFilterInject<Inc<AnimationDirectionsComponent, LookingDirection>, Exc<DisabledComponent>> directionFilter = default;
        readonly EcsFilterInject<Inc<AnimationDirectionsComponent, AnimationsComponent>, Exc<DisabledComponent>> animationFilter = default;
        
        public void OnEntityCreated(World world, Entity entity)
        {
            if (world.HasComponent<AnimationDirectionsComponent>(entity))
            {
                // var animations = world.GetComponent<AnimationsComponent>(entity);
                ref var animationDirections = ref world.GetComponent<AnimationDirectionsComponent>(entity);

                // CACHE ANIMATIONS PER DIRECTION

                if (world.HasComponent<StartingAnimationComponent>(entity))
                {
                    var startingAnimation = world.GetComponent<StartingAnimationComponent>(entity);
                    ProcessStartAnimation(ref animationDirections, startingAnimation);
                    world.RemoveComponent<StartingAnimationComponent>(entity);
                }
            }
        }
        
        public void OnEntityDestroyed(World world, Entity entity)
        {
            if (world.HasComponent<AnimationDirectionsComponent>(entity))
            {
                ref var animations = ref world.GetComponent<AnimationDirectionsComponent>(entity);
                // animations.cachedAnimations = null;
                animations.animationsAsset = null;
            }
        }

        public static void ProcessStartAnimation(ref AnimationDirectionsComponent animationsDirections, StartingAnimationComponent startingAnimationComponent)
        {
            var animationName = string.Empty;
                
            if (startingAnimationComponent.startingAnimationType == StartingAnimationComponent.StartingAnimationType.Name)
            {
                animationName = startingAnimationComponent.name;
            } else if (startingAnimationComponent.startingAnimationType == StartingAnimationComponent.StartingAnimationType.Random)
            {
                throw new NotImplementedException();
                
                // ideally use cached animations (no directions yet) from directions component.
            }

            var startingFrame = 0;
                
           // var animationDefinition = animations.animationsAsset.animations[animation];

            // if (startingAnimationComponent.randomizeStartFrame)
            // {
            //     startingFrame = UnityEngine.Random.Range(0, animationDefinition.TotalFrames);
            // }
            // else
            // {
            //     startingFrame = Mathf.FloorToInt(startingAnimationComponent.alpha * animationDefinition.TotalFrames);
            // }

            animationsDirections.Play(animationName, startingFrame, startingAnimationComponent.loop ? -1 : 1);
                
            // if (startingAnimationComponent.randomizeStartFrame)
            // {
            //     // also randomize current frame time, in case frames are bigger than 1fps.
            //     animations.currentTime =
            //         UnityEngine.Random.Range(0, animationDefinition.frames[startingFrame].time);
            //     animations.playingTime = animations.currentTime;
            // }
        }
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in startingAnimationFilter.Value)
            {
                var worldEntity = world.GetEntity(entity);
                
                ref var animations = ref startingAnimationFilter.Pools.Inc1.Get(entity);
                ref var startingAnimation = ref startingAnimationFilter.Pools.Inc2.Get(entity);
                
                ProcessStartAnimation(ref animations, startingAnimation);
                
                world.RemoveComponent<StartingAnimationComponent>(worldEntity);
            }
            
            // copy direction from looking direction
            
            foreach (var entity in directionFilter.Value)
            {
                ref var animationDirections = ref directionFilter.Pools.Inc1.Get(entity);
                ref var lookingDirection = ref directionFilter.Pools.Inc2.Get(entity);

                if (animationDirections.directionSource == AnimationDirectionsComponent.DirectionSource.LookingDirection)
                {
                    animationDirections.direction = GamePerspective.ProjectFromWorld(lookingDirection.value);
                }
            }
            
            foreach (var entity in animationFilter.Value)
            {
                ref var animationDirections = ref animationFilter.Pools.Inc1.Get(entity);
                ref var animations = ref animationFilter.Pools.Inc2.Get(entity);

                var command = animationDirections.pendingCommand;
                
                if (command.command == AnimationCommand.Command.Play)
                {
                    // process here
                    // get directional animation
                    // TODO: CACHE DIRECTIONS TOO
                    var animationName = animationDirections.animationsAsset.GetDirectionalAnimation(command.name,
                        animationDirections.direction);
                    var animationIndex = animationDirections.animationsAsset.GetAnimationIndexByName(animationName);
                    
                    animations.Play(animationIndex, command.frame, command.loops);
                    
                    animationDirections.pendingCommand = new AnimationCommand()
                    {
                        command = AnimationCommand.Command.None
                    };
                }
                else
                {
                    var animationName = animationDirections.animationsAsset.GetDirectionalAnimation(animationDirections.currentAnimation,
                        animationDirections.direction);

                    if (!animations.IsPlaying(animationName))
                    {
                        var animationIndex = animationDirections.animationsAsset.GetAnimationIndexByName(animationName);
                        animations.currentAnimation = animationIndex;
                    }
                }
            }
            
            // if pending animation change, then get animation real name, call animation play in the other component
        }
    }
}