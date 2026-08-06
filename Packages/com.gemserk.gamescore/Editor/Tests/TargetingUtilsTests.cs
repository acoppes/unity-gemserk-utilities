using Game.Components;
using Game.Utilities;
using MyBox;
using NUnit.Framework;
using UnityEngine;

namespace Game.Editor.Tests
{
    public class TargetingUtilsTests
    {
        [Test]
        public void Test_ValidateDirection_XZ()
        {
            var runtimeTargetingParameters = new RuntimeTargetingParameters()
            {
                direction = new Vector3(0f, 0f, 1f),
                position = new Vector3(10, 0, 10),
                alliedPlayersBitmask = 1,
                rangeMultiplier = 1f,
                filter = new TargetingFilter()
                {
                    aliveType = HealthComponent.AliveType.Alive,
                    angle = new MinMaxFloat(0, 20),
                    angleType = TargetingFilter.CheckDistanceType.PlaneXZ,
                    distanceType = TargetingFilter.CheckDistanceType.Sphere,
                    playerAllianceType = PlayerAllianceType.Everything,
                    range = new MinMaxFloat(0, 2f)
                }
            };
            
            Assert.True(TargetingUtils.ValidateTarget(new Target()
            {
                player = 0,
                position = new Vector3(10, 0, 11),
                targetType = 1,
                aliveType = HealthComponent.AliveType.Alive
            }, runtimeTargetingParameters));
            
            Assert.False(TargetingUtils.ValidateTarget(new Target()
            {
                player = 0,
                position = new Vector3(10, 0, 9),
                targetType = 1,
                aliveType = HealthComponent.AliveType.Alive
            }, runtimeTargetingParameters));
        }
        
        [Test]
        public void Test_ValidateDirection_XY()
        {
            var runtimeTargetingParameters = new RuntimeTargetingParameters()
            {
                direction = new Vector3(0f, 1f, 0f),
                position = new Vector3(10, 10, 0),
                alliedPlayersBitmask = 1,
                rangeMultiplier = 1f,
                filter = new TargetingFilter()
                {
                    aliveType = HealthComponent.AliveType.Alive,
                    angle = new MinMaxFloat(0, 20),
                    angleType = TargetingFilter.CheckDistanceType.Sphere,
                    distanceType = TargetingFilter.CheckDistanceType.Sphere,
                    playerAllianceType = PlayerAllianceType.Everything,
                    range = new MinMaxFloat(0, 2f)
                }
            };
            
            Assert.True(TargetingUtils.ValidateTarget(new Target()
            {
                player = 0,
                position = new Vector3(10, 11, 0),
                targetType = 1,
                aliveType = HealthComponent.AliveType.Alive
            }, runtimeTargetingParameters));
            
            Assert.False(TargetingUtils.ValidateTarget(new Target()
            {
                player = 0,
                position = new Vector3(10, 9, 0),
                targetType = 1,
                aliveType = HealthComponent.AliveType.Alive
            }, runtimeTargetingParameters));
        }
        
        [Test]
        public void Test_TargetSize_ForDistance()
        {
            var runtimeTargetingParameters = new RuntimeTargetingParameters()
            {
                position = new Vector3(10, 10, 0),
                alliedPlayersBitmask = 1,
                rangeMultiplier = 1f,
                filter = new TargetingFilter()
                {
                    aliveType = HealthComponent.AliveType.None,
                    angleType = TargetingFilter.CheckDistanceType.Nothing,
                    distanceType = TargetingFilter.CheckDistanceType.Sphere,
                    playerAllianceType = PlayerAllianceType.Everything,
                    range = new MinMaxFloat(1f, 2f)
                }
            };
            
            Assert.False(TargetingUtils.ValidateTarget(new Target()
            {
                player = 0,
                position = new Vector3(12.1f, 10, 0),
                targetType = 1,
                size = 0
            }, runtimeTargetingParameters));
            
            Assert.True(TargetingUtils.ValidateTarget(new Target()
            {
                player = 0,
                position = new Vector3(14, 10, 0),
                targetType = 1,
                aliveType = HealthComponent.AliveType.Alive,
                size = 2
            }, runtimeTargetingParameters));
            
            Assert.False(TargetingUtils.ValidateTarget(new Target()
            {
                player = 0,
                position = new Vector3(10.5f, 10, 0),
                targetType = 1,
                aliveType = HealthComponent.AliveType.Alive,
                size = 0
            }, runtimeTargetingParameters));
            
            Assert.True(TargetingUtils.ValidateTarget(new Target()
            {
                player = 0,
                position = new Vector3(10.5f, 10, 0),
                targetType = 1,
                aliveType = HealthComponent.AliveType.Alive,
                size = 1f
            }, runtimeTargetingParameters));
        }
    }
}