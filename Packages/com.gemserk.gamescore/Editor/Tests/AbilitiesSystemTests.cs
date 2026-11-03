using System.Collections.Generic;
using Game.Components;
using Game.Components.Abilities;
using Game.Systems;
using Game.Utilities;
using Gemserk.Leopotam.Ecs;
using NUnit.Framework;
using UnityEngine;

namespace Game.Editor.Tests
{
    public class AbilitiesSystemTests
    {
        private World world;
        
        [SetUp]
        public void BeforeEach()
        {
            var gameObject = new GameObject("~TestWorldObject");
            world = gameObject.AddComponent<World>();
            gameObject.AddComponent<AbilitiesSystem>();
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
        public void Bug_TargetWithInvalidEntity_ShouldBeNotValidTarget()
        {
            var targetEntity = world.CreateEntity(null, null, e =>
            {
                e.Add(new TargetComponent()
                {
                    target = new Target()
                    {
                        targetType = (int) TargetType.TargetType1,
                        size = 1,
                        entity = e
                    }
                });
            });
            
            var abilitiesEntity = world.CreateEntity(null, null, e =>
            {
                e.Add(AbilitiesComponent.Create());
            });
            
            abilitiesEntity.Get<AbilitiesComponent>().Add(new Ability()
            {
                name = "attack",
                abilityTargets = new List<AbilityTarget>(),
                targeting = new TargetingFilter()
                {
                    playerAllianceType = PlayerAllianceType.Everything
                }
            });

            var ability = abilitiesEntity.Get<AbilitiesComponent>().GetAbility("attack");
            ability.CopyTarget(targetEntity.Get<TargetComponent>().target, true);
            ability.targetsLocked = true;
            
            world.FixedUpdate();

            Assert.IsTrue(ability.hasTargets);
            Assert.IsTrue(ability.abilityTargets[0].valid);

            world.DestroyEntity(targetEntity);
            
            world.FixedUpdate();
            
            Assert.IsTrue(ability.hasTargets);
            Assert.IsFalse(ability.abilityTargets[0].valid);
        }
    }
}