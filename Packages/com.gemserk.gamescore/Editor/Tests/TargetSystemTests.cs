using Game.Components;
using Game.Systems;
using Game.Utilities;
using Gemserk.Leopotam.Ecs;
using MyBox;
using NUnit.Framework;
using UnityEngine;

namespace Game.Editor.Tests
{
    public class TargetSystemTests
    {
        private World world;
        
        [SetUp]
        public void BeforeEach()
        {
            var gameObject = new GameObject("~TestWorldObject");
            world = gameObject.AddComponent<World>();
            gameObject.AddComponent<TargetSystem>();
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
        public void Bug_DisabledEntity_IsStillValidTarget()
        {
            var targetEntity = world.CreateEntity(null, null, e =>
            {
                e.Add(new TargetComponent()
                {
                    target = new Target()
                });
                e.Add(new HealthComponent()
                {
                    current = 100,
                    total = 100
                });
            });
            
            world.FixedUpdate();

            Assert.IsTrue(TargetingUtils.ValidateTarget(targetEntity.Get<TargetComponent>().target, new RuntimeTargetingParameters()
            {
                filter = new TargetingFilter()
                {
                    aliveType = HealthComponent.AliveType.Alive,
                    angleType = TargetingFilter.CheckDistanceType.Nothing,
                    distanceType = TargetingFilter.CheckDistanceType.Nothing,
                    playerAllianceType = PlayerAllianceType.Everything,
                    targetTypes = TargetType.Everything
                }
            }));
            
            targetEntity.Add(new DisabledComponent());
            
            world.FixedUpdate();
            
            Assert.IsFalse(TargetingUtils.ValidateTarget(targetEntity.Get<TargetComponent>().target, new RuntimeTargetingParameters()
            {
                filter = new TargetingFilter()
                {
                    aliveType = HealthComponent.AliveType.Alive,
                    angleType = TargetingFilter.CheckDistanceType.Nothing,
                    distanceType = TargetingFilter.CheckDistanceType.Nothing,
                    playerAllianceType = PlayerAllianceType.Everything,
                    targetTypes = TargetType.Everything
                }
            }));
        }
        
        [Test]
        public void Bug_DisabledEnabledEntity_ShouldKeepTargetType()
        {
            var targetEntity = world.CreateEntity(null, null, e =>
            {
                e.Add(new TargetComponent()
                {
                    target = new Target()
                    {
                        targetType = 2,
                        size = 10
                    }
                });
                e.Add(new HealthComponent()
                {
                    current = 100,
                    total = 100
                });
            });
            
            targetEntity.Add(new DisabledComponent());
            world.FixedUpdate();
            
            targetEntity.Remove<DisabledComponent>();
            world.FixedUpdate();

            Assert.AreEqual(2, targetEntity.Get<TargetComponent>().target.targetType);
            Assert.AreEqual(10, targetEntity.Get<TargetComponent>().target.size);
        }
        
        
    }
}