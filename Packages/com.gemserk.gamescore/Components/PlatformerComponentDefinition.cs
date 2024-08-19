using System;
using Gemserk.Leopotam.Ecs;
using UnityEngine.Serialization;

namespace Game.Components
{
    [Serializable]
    public struct PlatformerComponent : IEntityComponent
    {
        [FormerlySerializedAs("horizontalSpeed")] 
        public float speed;

        public float airHorizontalSpeed;

        [NonSerialized]
        public bool fallingConsumesGroundJump;
        
        [NonSerialized]
        public float baseSpeedBoost;

        [NonSerialized]
        public float extraSpeedBoost;

        [NonSerialized]
        public float fallingJumpGroundDetectionOffset;

        [NonSerialized]
        public float fallingTime;

        public float TotalSpeedBoost => baseSpeedBoost + extraSpeedBoost;

        [NonSerialized]
        public bool walking;

        public float GetSpeed(bool inGround)
        {
            return inGround ? speed * TotalSpeedBoost : airHorizontalSpeed * TotalSpeedBoost;
        }
    }
    
    public class PlatformerComponentDefinition : ComponentDefinitionBase
    {
        public PlatformerComponent platformerComponent;

        public bool fallingConsumesGroundJump = true;
        public float fallingJumpGroundDetectionOffset = 1f;
        public float baseSpeedBoost = 1.0f;

        public override void Apply(World world, Entity entity)
        {
            platformerComponent.fallingJumpGroundDetectionOffset = fallingJumpGroundDetectionOffset;
            platformerComponent.extraSpeedBoost = 0;
            platformerComponent.baseSpeedBoost = baseSpeedBoost;
            platformerComponent.fallingConsumesGroundJump = fallingConsumesGroundJump;
            world.AddComponent(entity, platformerComponent);
        }
    }
}