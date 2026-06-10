using System.Collections.Generic;
using System.Text.RegularExpressions;
using Game.Components;
using Game.Definitions;
using Game.Systems;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Game.Editor.Tests
{
    public class AnimationTests
    {
        private World world;
        
        [SetUp]
        public void BeforeEach()
        {
            var gameObject = new GameObject("~TestWorldObject");
            world = gameObject.AddComponent<World>();
            
            gameObject.AddComponent<AnimationSystem>();
            
            world.fixedUpdateParent = world.transform;
            
            world.Awake();
        }
        
        [TearDown]
        public void AfterEach()
        {
            world.OnDestroy();
            Object.DestroyImmediate(world.gameObject);
            world = null;
        }
        
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
            
            AnimationSystem.UpdateAnimation(1, ref animationComponent, 1.0f);
            
            // Assert.AreEqual(1, animationComponent.playingTime, 0.01f);
            Assert.AreEqual(1, animationComponent.totalPlayingTime, 0.01f);
            
            animationComponent.pauseTime = 0.5f;

            AnimationSystem.UpdateAnimation(1, ref animationComponent, 0.5f);
            AnimationSystem.UpdateAnimation(1, ref animationComponent, 0.5f);
            
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
                name = "Idle"
            });
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
         

            var directionsData = animationAsset.GetDirectionsData();
            
            Assert.IsTrue(directionsData.animations.ContainsKey("Walk"));
            Assert.IsTrue(directionsData.animations.ContainsKey("Idle"));
            
            Assert.AreEqual(3, directionsData.animations["Walk"].directions);
            Assert.AreEqual(1, directionsData.animations["Idle"].directions);
            
            Assert.AreEqual("Walk-0", directionsData.animations["Walk"].directionsList[0].animationName);

            var directionalAnimation = directionsData.GetDirectionalAnimation("Walk", new Vector2(1, 0));
            
            Assert.AreEqual("Walk-1", directionalAnimation.animationName);
            Assert.AreEqual(1, directionalAnimation.direction);
            Assert.AreEqual(2, directionalAnimation.animationIndex);
            
            directionalAnimation =directionsData.GetDirectionalAnimation("Idle", new Vector2(1, 0));
            Assert.AreEqual("Idle", directionalAnimation.animationName);
            Assert.AreEqual(0, directionalAnimation.direction);
            Assert.AreEqual(0, directionalAnimation.animationIndex);
        }

        [Test]
        public void Test_UpdateAnimation_WrongAnimationIndex()
        {
            var animationComponent = new AnimationsComponent();
            animationComponent.speed = 1;
            
            animationComponent.animationsAsset = ScriptableObject.CreateInstance<AnimationsAsset>();
            animationComponent.animationsAsset.name = "my animation asset";
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

            animationComponent.Play(5);
            
            AnimationSystem.UpdateAnimation(1, ref animationComponent, 0.5f);
            
            LogAssert.Expect(LogType.Error, new Regex(".*wrong animation index.*"));
            Assert.AreEqual(0f, animationComponent.currentTime, 0.01f);
        }
        
        [Test]
        public void Test_UpdateAnimation_WrongFrameIndex()
        {
            var entity = world.CreateEntity(null, null, e =>
            {
                var animationComponent = new AnimationsComponent
                {
                    speed = 1,
                    animationsAsset = ScriptableObject.CreateInstance<AnimationsAsset>()
                };

                animationComponent.animationsAsset.name = "my animation asset";
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

                e.Add(animationComponent);
            });

            entity.Get<AnimationsComponent>().Play("Idle");
            entity.Get<AnimationsComponent>().currentFrame = 15;
            
            world.FixedUpdate();
            
            // AnimationSystem.UpdateAnimation(1, ref animationComponent, 0.5f);
            
            LogAssert.Expect(LogType.Error, new Regex(".*wrong frame index.*"));
            Assert.AreEqual(0f, entity.Get<AnimationsComponent>().currentTime, 0.01f);
        }
    }
}