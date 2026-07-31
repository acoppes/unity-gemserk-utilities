using System;
using System.Collections.Generic;
using Game.Components;
using Game.Systems;
using Game.Utilities;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using MyBox;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Editor.Tests
{
    public class MockEntityDefinition : IEntityDefinition
    {
        private Action<Entity> createCallback;

        public MockEntityDefinition(Action<Entity> createCallback)
        {
            this.createCallback = createCallback;
        }

        public void Apply(World world, Entity entity)
        {
            createCallback(entity);
        }
    }
    
    public class EffectSystemTests
    {
        private World world;

        [SetUp]
        public void BeforeEach()
        {
            var gameObject = new GameObject();
            world = gameObject.AddComponent<World>();

            gameObject.AddComponent<EffectSystem>();
            
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
        public void Test_ApplyEffect_FactorInverted()
        {
            var damageable = world.CreateEntity(null, null, (Entity e) =>
            {
                e.Add(new HealthComponent()
                {
                    damages = new List<HealthChangeData>()
                });
                e.Add(new TargetComponent()
                {
                    target = new Target()
                    {
                        entity = e
                    }
                });
            });
            
            var damage = world.CreateEntity(null, null, (Entity e) =>
            {
                e.Add(new EffectsComponent()
                {
                    target = damageable.Get<TargetComponent>().target,
                    factor = 1f,
                    valueMultiplier = 1f,
                    effects = new List<Effect>()
                    {
                        new Effect()
                        {
                            maxValue = 20,
                            minValue = 0,
                            targetType = Effect.TargetType.Target,
                            customEffect = null,
                            valueCalculationType = Effect.ValueCalculationType.BasedOnFactor,
                            type = Effect.EffectType.Damage
                        }
                    }
                });
            });
            
            world.FixedUpdate();
            
            Assert.That(damageable.Get<HealthComponent>().damages.Count, Is.EqualTo(1));
            Assert.That(damageable.Get<HealthComponent>().damages[0].value, Is.EqualTo(20));
        }
        
        [Test]
        public void Test_AreaEffect_DistanceInverseFactor()
        {
            var damageable1 = world.CreateEntity(null, null, (Entity e) =>
            {
                e.Add(new TargetComponent()
                {
                    target = new Target()
                    {
                        entity = e,
                        aliveType = HealthComponent.AliveType.Alive,
                        targetType = (int) TargetType.TargetType0,
                        healthFactor = 1,
                        position = new Vector3(0, 0, 0)
                    }
                });
            });
            
            var damageable2 = world.CreateEntity(null, null, (Entity e) =>
            {
                e.Add(new TargetComponent()
                {
                    target = new Target()
                    {
                        entity = e,
                        aliveType = HealthComponent.AliveType.Alive,
                        targetType = (int) TargetType.TargetType0,
                        healthFactor = 1,
                        position = new Vector3(10, 0, 0)
                    }
                });
            });

            var damageEntityDefinition = new MockEntityDefinition((Entity e) =>
            {
                e.Add(new PositionComponent());
                e.Add(new PlayerComponent());
                e.Add(new EffectsComponent()
                {
                    effects = new List<Effect>()
                    {
                        new Effect()
                        {
                            maxValue = 20,
                            minValue = 0,
                            targetType = Effect.TargetType.Target,
                            customEffect = null,
                            valueCalculationType = Effect.ValueCalculationType.BasedOnFactor,
                            type = Effect.EffectType.Damage
                        }
                    }
                });
            });
            
            var areaEffect = world.CreateEntity(null, null, (Entity e) =>
            {
                e.Add(new AreaEffectComponent()
                {
                    direction = new Vector3(1,0,0),
                    effectValueMultiplier = 1,
                    rangeMultiplier = 1,
                    targeting =  new TargetingFilter()
                    {
                        aliveType = HealthComponent.AliveType.Alive,
                        angleType = TargetingFilter.CheckDistanceType.Nothing,
                        distanceType = TargetingFilter.CheckDistanceType.Nothing,
                        playerAllianceType = PlayerAllianceType.Everything,
                        range = new MinMaxFloat(0, 10),
                        targetTypes = TargetType.Everything
                    },
                    effectDefinitions = new List<IEntityDefinition>()
                    {
                        damageEntityDefinition
                    }
                });
                e.Add(new DestroyableComponent());
                e.Add(new PlayerComponent()
                {
                    player = 0
                });
                e.Add(new PositionComponent()
                {
                    value = new Vector3(0, 0, 0)
                });
            });
            
            world.FixedUpdate();

            var effectsFilter = world.GetFilter<EffectsComponent>().End();
            Assert.That(effectsFilter.GetEntitiesCount(), Is.EqualTo(2));

            var enumerator = effectsFilter.GetEnumerator();
            enumerator.MoveNext();
            
            var effectEntity = enumerator.Current;
            var effectEntityInstance =  world.GetEntity(effectEntity);
            
            Assert.That(effectEntityInstance.Get<EffectsComponent>().target, Is.SameAs(damageable1.Get<TargetComponent>().target));
            Assert.That(effectEntityInstance.Get<EffectsComponent>().factor, Is.EqualTo(1f));
            
            enumerator.MoveNext();
            
            effectEntity = enumerator.Current;
            effectEntityInstance =  world.GetEntity(effectEntity);
            
            Assert.That(effectEntityInstance.Get<EffectsComponent>().target, Is.SameAs(damageable2.Get<TargetComponent>().target));
            Assert.That(effectEntityInstance.Get<EffectsComponent>().factor, Is.EqualTo(0f));
            
            enumerator.Dispose();

            // Assert.That(damageable.Get<HealthComponent>().damages.Count, Is.EqualTo(1));
            // Assert.That(damageable.Get<HealthComponent>().damages[0].value, Is.EqualTo(20));
        }
    }
}