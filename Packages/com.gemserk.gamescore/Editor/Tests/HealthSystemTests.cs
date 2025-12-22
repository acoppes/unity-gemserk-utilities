using Game.Components;
using Game.Systems;
using NUnit.Framework;

namespace Game.Editor.Tests
{
    public class HealthSystemTests
    {
        [Test]
        public void Health_ProcessedDamage_Positive()
        {
            var health = new HealthComponent
            {
                total = 100,
                current = 50
            };

            var result = HealthSystem.ProcessDamage(ref health, new DamageData()
            {
                value = 15
            });
            
            Assert.AreEqual(15, result.value, 0.1f);
            Assert.AreEqual(35, health.current, 0.1f);
        }
        
        [Test]
        public void Health_ProcessedDamage_WhenNegative()
        {
            var health = new HealthComponent
            {
                total = 100,
                current = 25
            };

            var result = HealthSystem.ProcessDamage(ref health, new DamageData()
            {
                value = 100
            });
            
            Assert.AreEqual(25, result.value, 0.1f);
            Assert.AreEqual(0, health.current, 0.1f);
            
            var result2 = HealthSystem.ProcessDamage(ref health, new DamageData()
            {
                value = 25
            });
            
            Assert.AreEqual(0, result2.value, 0.1f);
            Assert.AreEqual(0, health.current, 0.1f);
        }
        
        [Test]
        public void NormalDamage_WhenNoDamageResistance()
        {
            var health = new HealthComponent
            {
                total = 50,
                current = 50
            };

            HealthSystem.ProcessDamage(ref health, new DamageData()
            {
                value = 15
            });
            
            Assert.AreEqual(35, health.current, 0.1f);
        }
        
        [Test]
        public void ReducedDamageHalf_WhenDamageResistance()
        {
            var health = new HealthComponent
            {
                total = 50,
                current = 50,
                damageResistance = 0.5f
            };

            HealthSystem.ProcessDamage(ref health, new DamageData()
            {
                value = 20
            });
            
            Assert.AreEqual(40, health.current, 0.1f);

            health.damageResistance = 1f;
            HealthSystem.ProcessDamage(ref health, new DamageData()
            {
                value = 20
            });
            
            Assert.AreEqual(40, health.current, 0.1f);
        }
        
        [Test]
        public void DamageResult_AffectedByDamageResistance()
        {
            var health = new HealthComponent
            {
                total = 50,
                current = 50,
                damageResistance = 0.5f
            };

            var damageResult = HealthSystem.ProcessDamage(ref health, new DamageData()
            {
                value = 20
            });
            
            Assert.AreEqual(10, damageResult.value, 0.1f);
        }
    }
}