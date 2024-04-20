using System;
using System.Collections.Generic;
using Game.Definitions;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Game.Components
{
    [Serializable]
    public class AnimationFrame
    {
        public Sprite sprite;

        public float time;
        
        // public Sprite fxSprite;

        // public List<string> events = new ();
        // public bool HasEvents => events.Count > 0;
    }
    
    [Serializable]
    public class AnimationDefinition
    {
        public const float DefaultFrameRate = 15.0f;
        
        public string name;
        public float duration;
        
        public List<AnimationFrame> frames = new ();
        
        public int TotalFrames => frames.Count;
    }

    public struct StartingAnimationComponent : IEntityComponent
    {
        public enum StartingAnimationType
        {
            None = 0,
            Name = 1,
            Random = 2
        }

        public StartingAnimationType startingAnimationType;
        public bool randomizeStartFrame;
        public float alpha;
        public string name;
        public bool loop;
    }

    public struct AnimationComponent : IEntityComponent
    {
        public const int NoAnimation = -1;
        public const int NoFrame = -1;
        
        // public delegate void OnAnimatorEventHandler(AnimationComponent animationComponent, int animation);
        // public delegate void OnAnimationEventHandler(AnimationComponent animationComponent, int animation, int frame);
        
        public enum State
        {
            Completed,
            Playing
        }
        
        public AnimationsAsset animationsAsset;
        public SpritesMetadata metadata;

        public int currentAnimation;
        public int currentFrame;
        public float currentTime;
        public int loops;
        public State state;
        public bool paused;

        public float speed;

        public float playingTime;
        public float totalPlayingTime;

        public float pauseTime;

        public bool isCompleted => state == State.Completed;

        // public event OnAnimatorEventHandler onStart;
        // public event OnAnimatorEventHandler onComplete;
        // public event OnAnimatorEventHandler onCompletedLoop;
        // public event OnAnimationEventHandler onEvent;
        
        // public bool onStartEventPending;
        
        public IDictionary<string, int> cachedAnimations;
        
        public float GetCurrentAnimationFactor()
        {
            if (currentAnimation != NoAnimation && currentFrame != NoFrame)
            {
                var animation = animationsAsset.animations[currentAnimation];

                if (animation.duration > 0)
                {
                    return playingTime / animation.duration;
                }
                
                return currentFrame / (float) animation.TotalFrames;
            }

            return 0f;
        }
        
        public void Play(int animation, int startFrame, bool loop)
        {
            Play(animation, startFrame, loop ? -1 : 0);
        }
        
        public void Play(int animation, int startFrame, int loops = -1)
        {
            currentAnimation = animation;
            currentFrame = startFrame;
            currentTime = 0;
            playingTime = 0;
            totalPlayingTime = 0;
            this.loops = loops;
            state = State.Playing;
            
            // onStartEventPending = true;
        }
        
        public void Play(int animation, int loops = -1)
        {
            Play(animation, 0, loops);
        }

        public void Play(string animation, int loops = -1)
        {
            var animationIndex = cachedAnimations[animation];
            // Assert.IsTrue(animationIndex >= 0, $"Couldn't find {animation}");
            Play(animationIndex, loops);
        }

        public bool IsPlaying(string animationName)
        {
            return currentAnimation == cachedAnimations[animationName];
        }

        public bool IsPlaying(int animation)
        {
            return currentAnimation == animation;
        }

        public int GetAnimationIndex(string animationName)
        {
            return cachedAnimations[animationName];
        }

        public bool HasAnimation(string animationName)
        {
            return cachedAnimations.ContainsKey(animationName);
        }

        public AnimationFrame GetFrame(int animation, int frame)
        {
            return animationsAsset.animations[animation].frames[frame];
        }
        
        // public void OnStart()
        // {
        //     onStart?.Invoke(this, currentAnimation);
        // }
        // public void OnComplete()
        // {
        //     onComplete?.Invoke(this, currentAnimation);
        // }
        //
        // public void OnCompletedLoop()
        // {
        //     onCompletedLoop?.Invoke(this, currentAnimation);
        // }
        //
        // public void OnEvent()
        // {
        //     onEvent?.Invoke(this, currentAnimation, currentFrame);
        // }
    }
}