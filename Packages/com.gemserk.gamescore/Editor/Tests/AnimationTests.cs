using System.Collections.Generic;
using Game.Components;
using Game.Definitions;
using Game.Systems;
using Gemserk.Utilities;
using NUnit.Framework;
using UnityEngine;

namespace Game.Editor.Tests
{
    public class AnimationTests
    {
        [Test]
        public void Test_TotalAnimationTime_IgnorePause()
        {
            var animationComponent = new AnimationComponent();
            animationComponent.speed = 1;
            
            animationComponent.animationsAsset = ScriptableObject.CreateInstance<AnimationsAsset>();
            animationComponent.animationsAsset.animations.Add(new AnimationDefinition()
            {
                name = "Idle",
                frames = new List<AnimationFrame>()
                {
                    new AnimationFrame()
                    {
                        time = 1f / 30f,
                        sprite = null
                    }
                }
            });
            
            animationComponent.Play(0);
            
            AnimationSystem.UpdateAnimation(ref animationComponent, 1.0f);
            
            Assert.AreEqual(1, animationComponent.playingTime, 0.01f);
            Assert.AreEqual(1, animationComponent.totalPlayingTime, 0.01f);
            
            animationComponent.pauseTime = 0.5f;

            AnimationSystem.UpdateAnimation(ref animationComponent, 0.5f);
            AnimationSystem.UpdateAnimation(ref animationComponent, 0.5f);
            
            Assert.AreEqual(1.5f, animationComponent.playingTime, 0.01f);
            Assert.AreEqual(2f, animationComponent.totalPlayingTime, 0.01f);
        }

        [Test]
        public void Test_GetDirectionalAnimation_NoDirections()
        {
            var animationAsset = ScriptableObject.CreateInstance<AnimationsAsset>();
            animationAsset.animations.Add(new AnimationDefinition()
            {
                name = "Walk"
            });

            var directionalName = animationAsset.GetDirectionalAnimation("Walk", new Vector2(1, 0));
            Assert.AreEqual("Walk", directionalName);
        }
        
        [Test]
        public void Test_GetDirectionalAnimation_WithDirections()
        {
            var animationAsset = ScriptableObject.CreateInstance<AnimationsAsset>();
            animationAsset.animations.Add(new AnimationDefinition()
            {
                name = "Walk-0"
            });
            animationAsset.animations.Add(new AnimationDefinition()
            {
                name = "Walk-1"
            });
            animationAsset.animations.Add(new AnimationDefinition()
            {
                name = "Walk-2"
            });

            Assert.AreEqual("Walk-0", animationAsset.GetDirectionalAnimation("Walk", new Vector2(0, -1)));
            Assert.AreEqual("Walk-1", animationAsset.GetDirectionalAnimation("Walk", new Vector2(1, 0)));
            Assert.AreEqual("Walk-2", animationAsset.GetDirectionalAnimation("Walk", Vector2.right.Rotate(88f * Mathf.Deg2Rad)));
        }

    }
}