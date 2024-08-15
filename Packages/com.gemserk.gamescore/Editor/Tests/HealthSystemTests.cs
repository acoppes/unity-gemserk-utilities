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
            Assert.AreEqual(-75, health.current, 0.1f);
            
            var result2 = HealthSystem.ProcessDamage(ref health, new DamageData()
            {
                value = 25
            });
            
            Assert.AreEqual(0, result2.value, 0.1f);
            Assert.AreEqual(-100, health.current, 0.1f);
        }
        
    }
}