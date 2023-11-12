using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Game.Components;
using Game.Definitions;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.Systems
{
    public class AnimationSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler, IEntityDestroyedHandler, IEcsDestroySystem
    {
        readonly EcsFilterInject<Inc<AnimationComponent, StartingAnimationComponent>, Exc<DisabledComponent>> startingAnimationFilter = default;
        readonly EcsFilterInject<Inc<AnimationComponent>, Exc<DisabledComponent>> animationFilter = default;

        private IDictionary<AnimationsAsset, IDictionary<string, int>> cachedAnimationsPerAsset = 
            new Dictionary<AnimationsAsset, IDictionary<string, int>>();
        
        public void Destroy(EcsSystems systems)
        {
            cachedAnimationsPerAsset.Clear();
        }
        
        public void OnEntityCreated(World world, Entity entity)
        {
            if (world.HasComponent<AnimationComponent>(entity))
            {
                ref var animations = ref world.GetComponent<AnimationComponent>(entity);
                var asset = animations.animationsAsset;
                if (asset != null)
                {
                    if (!cachedAnimationsPerAsset.ContainsKey(asset))
                    {
                        cachedAnimationsPerAsset[asset] = asset.GetAnimationsByNameDictionary();
                    }
                    animations.cachedAnimations = cachedAnimationsPerAsset[asset];
                }

                if (world.HasComponent<StartingAnimationComponent>(entity))
                {
                    var startingAnimation = world.GetComponent<StartingAnimationComponent>(entity);
                    ProcessStartAnimation(ref animations, startingAnimation);
                    world.RemoveComponent<StartingAnimationComponent>(entity);
                }
            }
        }
        
        public void OnEntityDestroyed(World world, Entity entity)
        {
            if (world.HasComponent<AnimationComponent>(entity))
            {
                ref var animations = ref world.GetComponent<AnimationComponent>(entity);
                animations.cachedAnimations = null;
                animations.animationsAsset = null;
            }
        }

        public static void ProcessStartAnimation(ref AnimationComponent animations, StartingAnimationComponent startingAnimationComponent)
        {
            var animation = 0;
                
            if (startingAnimationComponent.startingAnimationType == StartingAnimationComponent.StartingAnimationType.Name)
            {
                animation = animations.cachedAnimations[startingAnimationComponent.name];
            } else if (startingAnimationComponent.startingAnimationType == StartingAnimationComponent.StartingAnimationType.Random)
            {
                animation = UnityEngine.Random.Range(0, animations.animationsAsset.animations.Count);
            }

            var startingFrame = 0;
                
            var animationDefinition = animations.animationsAsset.animations[animation];

            if (startingAnimationComponent.randomizeStartFrame)
            {
                startingFrame = UnityEngine.Random.Range(0, animationDefinition.TotalFrames);
            }
            else
            {
                startingFrame = Mathf.FloorToInt(startingAnimationComponent.alpha * animationDefinition.TotalFrames);
            }

            animations.Play(animation, startingFrame, startingAnimationComponent.loop);
                
            if (startingAnimationComponent.randomizeStartFrame)
            {
                // also randomize current frame time, in case frames are bigger than 1fps.
                animations.currentTime =
                    UnityEngine.Random.Range(0, animationDefinition.frames[startingFrame].time);
                animations.playingTime = animations.currentTime;
            }
        }
        
        public void Run(EcsSystems systems)
        {
            var animationDt = dt;      
            
            // just in case it cached was added later...
            // foreach (var entity in animationFilter.Value)
            // {
            //     ref var animations = ref animationFilter.Pools.Inc1.Get(entity);
            //     if (animations.cachedAnimations == null && animations.animationsAsset != null)
            //     {
            //         var asset = animations.animationsAsset;
            //         if (!cachedAnimationsPerAsset.ContainsKey(asset))
            //         {
            //             cachedAnimationsPerAsset[asset] = asset.GetAnimationsByNameDictionary();
            //         }
            //         animations.cachedAnimations = cachedAnimationsPerAsset[asset];
            //     }
            // }
            
            foreach (var entity in startingAnimationFilter.Value)
            {
                var worldEntity = world.GetEntity(entity);
                
                ref var animations = ref startingAnimationFilter.Pools.Inc1.Get(entity);
                ref var startingAnimation = ref startingAnimationFilter.Pools.Inc2.Get(entity);
                
                ProcessStartAnimation(ref animations, startingAnimation);
                
                world.RemoveComponent<StartingAnimationComponent>(worldEntity);
            }
            
            foreach (var entity in animationFilter.Value)
            {
                ref var animationComponent = ref animationFilter.Pools.Inc1.Get(entity);
                UpdateAnimation(ref animationComponent, animationDt);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UpdateAnimation(ref AnimationComponent animationComponent, float dt)
        {
            if (animationComponent.paused)
            {
                return;
            }
            
            var animationDt = dt * animationComponent.speed;
            
            if (animationComponent.state == AnimationComponent.State.Playing)
            {
                animationComponent.totalPlayingTime += animationDt;
            }

            if (animationComponent.pauseTime > 0)
            {
                animationComponent.pauseTime -= animationDt;
                return;
            }

            if (animationComponent.state == AnimationComponent.State.Playing)
            {
                // if (animationComponent.onStartEventPending)
                // {
                //     animationComponent.OnStart();
                //     animationComponent.onStartEventPending = false;
                // }

                var currentAnimation = animationComponent.animationsAsset.animations[animationComponent.currentAnimation];

                animationComponent.currentTime += animationDt;
                animationComponent.playingTime += animationDt;

                var currentFrame = currentAnimation.frames[animationComponent.currentFrame];
                
                // #if UNITY_EDITOR
                // Assert.AreNotApproximatelyEqual(currentFrame.time, 0.0f, "Invalid frame duration");
                // #endif
                
                while (animationComponent.currentTime >= currentFrame.time)
                {
                    currentFrame = currentAnimation.frames[animationComponent.currentFrame];
                    
                    // #if UNITY_EDITOR
                    // Assert.AreNotApproximatelyEqual(currentFrame.time, 0.0f, "Invalid frame duration");
                    // #endif
                    
                    // if (definition.frames != null && definition.frames.Count > 0 && definition.frames[animationComponent.currentFrame].HasEvents)
                    // {
                    //     animationComponent.OnEvent();
                    // }

                    animationComponent.currentTime -= currentFrame.time;
                    animationComponent.currentFrame++;

                    if (animationComponent.currentFrame >= currentAnimation.TotalFrames)
                    {
                        if (animationComponent.loops > 0)
                        {
                            animationComponent.loops -= 1;
                        }

                        // if (animationComponent.loops == -1)
                        // {
                        //     animationComponent.OnCompletedLoop();
                        // }

                        if (animationComponent.loops == 0)
                        {
                            animationComponent.state = AnimationComponent.State.Completed;
                            animationComponent.currentFrame = currentAnimation.TotalFrames - 1;
                            // animationComponent.OnComplete();
                            break;
                        }

                        animationComponent.currentFrame = 0;
                    }
                }
            }
        }

    }
}