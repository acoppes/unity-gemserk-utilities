using System;
using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MyBox;
using UnityEngine;

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
                animationDirections.metadata = animationDirections.animationsAsset.GetDirectionsData();

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
                var random = animationsDirections.metadata.animations.GetRandom();
                animationName = random.Key;
            }

            var startingFrame = 0;

            animationsDirections.directionalAnimation =  animationsDirections.metadata.GetDirectionalAnimation(animationName, animationsDirections.direction);

            var animationDefinition =
                animationsDirections.animationsAsset.animations[
                    animationsDirections.directionalAnimation.animationIndex];

            if (startingAnimationComponent.randomizeStartFrame)
            {
                startingFrame = UnityEngine.Random.Range(0, animationDefinition.TotalFrames);
            }
            else
            {
                startingFrame = Mathf.FloorToInt(startingAnimationComponent.alpha * animationDefinition.TotalFrames);
            }
                
            // if (startingAnimationComponent.randomizeStartFrame)
            // {
            //     // also randomize current frame time, in case frames are bigger than 1fps.
            //     animations.currentTime =
            //         UnityEngine.Random.Range(0, animationDefinition.frames[startingFrame].time);
            //     animations.playingTime = animations.currentTime;
            // }
            
            animationsDirections.Play(animationName, startingFrame, startingAnimationComponent.loop ? -1 : 1);
        }
        
        public void Run(EcsSystems systems)
        {
            foreach (var e in startingAnimationFilter.Value)
            {
                // var entity = world.GetEntity(e);
                
                ref var animations = ref startingAnimationFilter.Pools.Inc1.Get(e);
                ref var startingAnimation = ref startingAnimationFilter.Pools.Inc2.Get(e);
                
                ProcessStartAnimation(ref animations, startingAnimation);
                
                startingAnimationFilter.Pools.Inc2.Del(e);
                // world.RemoveComponent<StartingAnimationComponent>(entity);
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
                    animationDirections.directionalAnimation = animationDirections.metadata.GetDirectionalAnimation(command.name,
                        animationDirections.direction);
                    
                    animations.Play(animationDirections.directionalAnimation.animationIndex, command.frame, command.loops);
                    
                    animationDirections.pendingCommand = new AnimationCommand()
                    {
                        command = AnimationCommand.Command.None
                    };
                }
                else
                {
                    animationDirections.directionalAnimation = animationDirections.metadata.GetDirectionalAnimation(animationDirections.currentAnimation,
                        animationDirections.direction);

                    if (!animations.IsPlaying(animationDirections.directionalAnimation.animationIndex))
                    {
                        // NOTE: I just change the anim without restarting it, I assume it has the same frames count and/or durations.
                        animations.currentAnimation = animationDirections.directionalAnimation.animationIndex;
                    }
                }
            }
        }
    }
}