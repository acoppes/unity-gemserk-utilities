using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Game.Components;
using Game.Definitions;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class AnimationSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler, IEntityDestroyedHandler, IEcsDestroySystem
    {
        readonly EcsFilterInject<Inc<AnimationsComponent, StartingAnimationComponent>, Exc<DisabledComponent>> startingAnimationFilter = default;
        readonly EcsFilterInject<Inc<AnimationsComponent>, Exc<DisabledComponent>> animationFilter = default;

        private IDictionary<AnimationsAsset, IDictionary<string, int>> cachedAnimationsPerAsset = 
            new Dictionary<AnimationsAsset, IDictionary<string, int>>();
        
        public void Destroy(EcsSystems systems)
        {
            cachedAnimationsPerAsset.Clear();
        }
        
        public void OnEntityCreated(World world, Entity entity)
        {
            if (world.HasComponent<AnimationsComponent>(entity))
            {
                ref var animations = ref world.GetComponent<AnimationsComponent>(entity);
                var asset = animations.animationsAsset;
                if (asset)
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
            if (world.HasComponent<AnimationsComponent>(entity))
            {
                ref var animations = ref world.GetComponent<AnimationsComponent>(entity);
                animations.cachedAnimations = null;
                animations.animationsAsset = null;
            }
        }

        public static void ProcessStartAnimation(ref AnimationsComponent animations, StartingAnimationComponent startingAnimationComponent)
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
            
            foreach (var e in startingAnimationFilter.Value)
            {
                // var entity = world.GetEntity(e);
                
                ref var animations = ref startingAnimationFilter.Pools.Inc1.Get(e);
                ref var startingAnimation = ref startingAnimationFilter.Pools.Inc2.Get(e);
                
                ProcessStartAnimation(ref animations, startingAnimation);
                
                startingAnimationFilter.Pools.Inc2.Del(e);
                // world.RemoveComponent<StartingAnimationComponent>(entity);
            }
            
            foreach (var entity in animationFilter.Value)
            {
                ref var animationComponent = ref animationFilter.Pools.Inc1.Get(entity);
                UpdateAnimation(ref animationComponent, animationDt);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UpdateAnimation(ref AnimationsComponent animations, float dt)
        {
            if (animations.paused)
            {
                return;
            }
            
            var animationDt = dt * animations.speed;
            
            if (animations.state == AnimationsComponent.State.Playing)
            {
                animations.totalPlayingTime += animationDt;
            }

            if (animations.pauseTime > 0)
            {
                animations.pauseTime -= animationDt;
                return;
            }

            if (animations.state == AnimationsComponent.State.Playing)
            {
                // if (animationComponent.onStartEventPending)
                // {
                //     animationComponent.OnStart();
                //     animationComponent.onStartEventPending = false;
                // }

                var currentAnimation = animations.animationsAsset.animations[animations.currentAnimation];

                animations.currentAnimationDefinition = currentAnimation;

                animations.currentTime += animationDt;
                animations.playingTime += animationDt;

                var currentFrame = currentAnimation.frames[animations.currentFrame];
                
                // #if UNITY_EDITOR
                // Assert.AreNotApproximatelyEqual(currentFrame.time, 0.0f, "Invalid frame duration");
                // #endif
                
                while (animations.currentTime >= currentFrame.time)
                {
                    currentFrame = currentAnimation.frames[animations.currentFrame];
                    
                    // #if UNITY_EDITOR
                    // Assert.AreNotApproximatelyEqual(currentFrame.time, 0.0f, "Invalid frame duration");
                    // #endif
                    
                    // if (definition.frames != null && definition.frames.Count > 0 && definition.frames[animationComponent.currentFrame].HasEvents)
                    // {
                    //     animationComponent.OnEvent();
                    // }

                    animations.currentTime -= currentFrame.time;
                    animations.currentFrame++;

                    if (animations.currentFrame >= currentAnimation.TotalFrames)
                    {
                        if (animations.loops != 0)
                        {
                            if (animations.loops > 0)
                                animations.loops -= 1;
                            
                            if (currentAnimation.duration > 0)
                            {
                                animations.playingTime -= currentAnimation.duration;
                            }
                            else
                            {
                                animations.playingTime = 0;
                            }
                        }

                        // if (animationComponent.loops == -1)
                        // {
                        //     animationComponent.OnCompletedLoop();
                        // }

                        if (animations.loops == 0)
                        {
                            animations.state = AnimationsComponent.State.Completed;
                            animations.currentFrame = currentAnimation.TotalFrames - 1;
                            
                            if (currentAnimation.duration > 0)
                                animations.playingTime = currentAnimation.duration;
                            
                            // animationComponent.OnComplete();
                            break;
                        }

                        animations.currentFrame = 0;
                    }
                }
            }
        }

    }
}