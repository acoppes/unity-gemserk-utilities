using System.Collections.Generic;
using System.Text.RegularExpressions;
using Game.Components;
using Game.Definitions;
using Game.Systems;
using Gemserk.Leopotam.Ecs;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Game.Editor.Tests
{
    public class CopyFromAnimationToModelSystemTests
    {
        private World world;
        
        [SetUp]
        public void BeforeEach()
        {
            var gameObject = new GameObject("~TestWorldObject");
            world = gameObject.AddComponent<World>();
            
            gameObject.AddComponent<CopyFromAnimationToModelSystem>();
            
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
        public void CopyFails_When_WrongAnimationIndex()
        {
            var animationsAsset = ScriptableObject.CreateInstance<AnimationsAsset>();
            animationsAsset.name = "my animation asset";
            animationsAsset.animations.Add(new AnimationDefinition()
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
            
            world.CreateEntity(null, null, entity =>
            {
                entity.Add(new AnimationsComponent()
                {
                    animationsAsset = animationsAsset,
                    currentAnimation = 10,
                    currentFrame = 0
                });
                entity.Add(new ModelInstanceComponent()
                {
                });
                entity.Add(new CopyAnimationCacheComponent()
                {
                    animation = -1,
                    frame = -1
                });
            });
            
            world.FixedUpdate();
            
            LogAssert.Expect(LogType.Error, new Regex(".*wrong animation index.*"));
        }
        
        [Test]
        public void CopyFails_When_WrongFrameIndex()
        {
            var animationsAsset = ScriptableObject.CreateInstance<AnimationsAsset>();
            animationsAsset.name = "my animation asset";
            animationsAsset.animations.Add(new AnimationDefinition()
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
            
            world.CreateEntity(null, null, entity =>
            {
                entity.Add(new AnimationsComponent()
                {
                    animationsAsset = animationsAsset,
                    currentAnimation = 0,
                    currentFrame = 100
                });
                entity.Add(new ModelInstanceComponent()
                {
                });
                entity.Add(new CopyAnimationCacheComponent()
                {
                    animation = -1,
                    frame = -1
                });
            });
            
            world.FixedUpdate();
            
            LogAssert.Expect(LogType.Error, new Regex(".*wrong frame index.*"));
        }
    }
}