using System;
using Gemserk.Leopotam.Ecs;
using UnityEngine.Scripting.APIUpdating;

namespace Beatemup.Queries
{
    [Serializable]
    // [MovedFrom(true, "Beatemup.Queries", "Beatemup")]
    [MovedFrom(true, "Beatemup.Queries", "Beatemup.Triggers")]
    public class PlayerQueryParameter : IQueryParameter
    {
        public int player;
        
        public bool MatchQuery(World world, Entity entity)
        {
            if (!world.HasComponent<PlayerComponent>(entity))
                return false;

            var playerComponent = world.GetComponent<PlayerComponent>(entity);

            return playerComponent.player == player;
        }

        public override string ToString()
        {
            return $"player:{player}";
        }
    }
}