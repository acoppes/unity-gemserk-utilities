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

    public struct AnimationCommand
    {
        public enum Command
        {
            None = 0,
            Play = 1
        }
    
        public Command command;
        public string name;
        public int frame;
        public int loops;
    }

    public struct DirectionalAnimationData
    {
        public int animationIndex;
        public int direction;
        public string animationName;
    }

    public class AnimationDirectionMetadata
    {
        public int directions => directionsList.Count;
        public List<DirectionalAnimationData> directionsList = new List<DirectionalAnimationData>();
    }
    
    public class AnimationsDirectionsMetadata
    {
        public Dictionary<string, AnimationDirectionMetadata> animations = new Dictionary<string, AnimationDirectionMetadata>();

        public void GetDirectionalAnimation(string animationName, Vector2 direction, out string animationDirectionName, out int animationDirection)
        {
            if (direction.x < 0)
            {
                direction.x *= -1;
            }
            
            var angle = Vector2.SignedAngle(Vector2.right, direction);
            GetDirectionalAnimation(animationName, angle, out animationDirectionName, out animationDirection);
        }
        
        public void GetDirectionalAnimation(string animationName, float angle, out string animationDirectionName, out int animationDirection)
        {
            var directions = animations[animationName].directions;
            
            if (directions > 0)
            {
                var anglePerDirection = 180 / directions;
                var currentAngle = -90;
                var nextAngle = currentAngle + anglePerDirection;
                
                for (var i = 0; i < directions; i++)
                {
                    if (angle >= currentAngle && angle <= nextAngle)
                    {
                        animationDirectionName = animations[animationName].directionsList[i].animationName;
                        animationDirection = animations[animationName].directionsList[i].direction;
                        return;
                        // return $"{animationName}-{i}";
                    }

                    currentAngle += anglePerDirection;
                    nextAngle += anglePerDirection;
                }
            }

            animationDirectionName = animationName;
            animationDirection = 0;
        }
    }

    public struct AnimationDirectionsComponent : IEntityComponent
    {
        public AnimationsAsset animationsAsset;

        public AnimationsDirectionsMetadata metadata;
        
        // cached anims
        // Dictionary<string, Animations[]> cachedDirections;

        public enum DirectionSource
        {
            LookingDirection = 0
        }

        public DirectionSource directionSource;
        
        public Vector2 direction;
        
        // public int currentDirection;
        public string currentAnimation;

        // READONLY
        public int currentDirectionIndex;
        public string currentDirectionalAnimation;

        // public int loops;
        public AnimationCommand pendingCommand;

        // allow changing animation during play
        // public bool updatesDirectionDuringPlay;
        
        public void Play(string animation, int loops = -1) // updatesDuringPlay = false)
        {
            Play(animation, 0, loops);
        }
        
        public void Play(string animation, int frame, int loops) // updatesDuringPlay = false)
        {
            currentAnimation = animation;
            
            // stores animation to be played
            pendingCommand = new AnimationCommand()
            {
                command = AnimationCommand.Command.Play,
                loops = loops,
                name = animation,
                frame = frame
            };
        }

        public bool IsPlaying(string animation)
        {
            // calculates direction, delegates to animation component?
            return animation.Equals(currentAnimation, StringComparison.OrdinalIgnoreCase);
        }
    }

    public struct AnimationsComponent : IEntityComponent
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

        // public AnimationCommand pendingCommand;
        
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

            // pendingCommand = new AnimationCommand
            // {
            //     command = AnimationCommand.Command.Play,
            //     name = animation,
            //     loops = loops,
            //     frame = 0
            // };
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