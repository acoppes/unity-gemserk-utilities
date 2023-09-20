using Game.Components;
using Game.Queries;
using Gemserk.Leopotam.Ecs;
using NUnit.Framework;
using UnityEngine;

namespace Game.Editor.Tests
{
    public class UnitTypeQueryTests
    {
        [Test]
        public void Test_UnitTypeParameter_MatchTypes()
        {
            var worldObject = new GameObject();
            var world = worldObject.AddComponent<World>();
            world.Init();

            var entity = world.CreateEntity();
            entity.Add(new UnitTypeComponent()
            {
                unitType = 1 << 3
            });
            
            Assert.IsFalse(new UnitTypeParameter(1 << 0).MatchQuery(entity));
            Assert.IsTrue(new UnitTypeParameter(1 << 3).MatchQuery(entity));
            Assert.IsTrue(new UnitTypeParameter(1 << 3 | 1 << 0).MatchQuery(entity));
        }
    }
}