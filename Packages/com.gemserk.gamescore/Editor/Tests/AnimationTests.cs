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
            var animationComponent = new AnimationsComponent();
            animationComponent.speed = 1;
            
            animationComponent.animationsAsset = ScriptableObject.CreateInstance<AnimationsAsset>();
            animationComponent.animationsAsset.animations.Add(new AnimationDefinition()
            {
                name = "Idle",
                duration = 1f,
                frames = new List<AnimationFrame>()
                {
                    new AnimationFrame()
                    {
                        time = 1f,
                        sprite = null
                    }
                }
            });
            
            animationComponent.Play(0);
            
            AnimationSystem.UpdateAnimation(ref animationComponent, 1.0f);
            
            // Assert.AreEqual(1, animationComponent.playingTime, 0.01f);
            Assert.AreEqual(1, animationComponent.totalPlayingTime, 0.01f);
            
            animationComponent.pauseTime = 0.5f;

            AnimationSystem.UpdateAnimation(ref animationComponent, 0.5f);
            AnimationSystem.UpdateAnimation(ref animationComponent, 0.5f);
            
            // Assert.AreEqual(1.5f, animationComponent.playingTime, 0.01f);
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
        
        [Test]
        public void Test_GetDirectionsMetadata()
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
            animationAsset.animations.Add(new AnimationDefinition()
            {
                name = "Idle"
            });

            var directionsData = animationAsset.GetDirectionsData();
            
            Assert.IsTrue(directionsData.animations.ContainsKey("Walk"));
            Assert.IsTrue(directionsData.animations.ContainsKey("Idle"));
            
            Assert.AreEqual(3, directionsData.animations["Walk"].directions);
            Assert.AreEqual(1, directionsData.animations["Idle"].directions);
            
            Assert.AreEqual("Walk-0", directionsData.animations["Walk"].animationNames[0]);

            string directionName;
            var directionIndex = -1;

            directionsData.GetDirectionalAnimation("Walk", new Vector2(1, 0), out directionName, out directionIndex);
            Assert.AreEqual("Walk-1", directionName);
            Assert.AreEqual(1, directionIndex);
            
            directionsData.GetDirectionalAnimation("Idle", new Vector2(1, 0), out directionName, out directionIndex);
            Assert.AreEqual("Idle", directionName);
            Assert.AreEqual(0, directionIndex);
        }

    }
}