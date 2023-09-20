using Game.Components;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Game.Utilities
{
    public class Target
    {
        public Entity entity;
        
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