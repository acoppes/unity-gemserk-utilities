using System;
using Game.Components;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Game.Utilities
{
    [Flags]
    public enum TargetType
    {
        TargetType0 = 1 << 0,
        TargetType1 = 1 << 1,
        TargetType2 = 1 << 2,
        TargetType3 = 1 << 3,
        TargetType4 = 1 << 4,
        TargetType5 = 1 << 5,
        TargetType6 = 1 << 6,
        TargetType7 = 1 << 7,
        
        // AllTargetTypes = TargetType0 | TargetType1 | TargetType2 | TargetType3 | TargetType4 | TargetType5 | TargetType6 | TargetType7,
    }
    
    public class Target
    {
        public Entity entity;

        public TargetType targetType;
        public int player;
        
        public int playerBitmask => 1 << player;
        public int alliedPlayersBitmask => PlayerAllianceExtensions.GetAlliedPlayers(player);
        
        // public int enemyPlayers;
        
        public Vector3 position;
        public Vector3 velocity;

        public float healthFactor;
        public HealthComponent.AliveType aliveType = HealthComponent.AliveType.None;
        
        public bool targeted;
    }

    public static class TargetExtensions
    {
        public static Vector3 GetEstimatedPosition(this Target target, float duration, float factor = 1.0f)
        {
            return target.position + target.velocity * (duration * factor);
        }
    }
}