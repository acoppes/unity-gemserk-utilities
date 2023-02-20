using Gemserk.Leopotam.Ecs;

namespace Gemserk.Triggers.Queries
{
    public class PlayerQueryParameter : QueryParameterBase
    {
        public int player;
        
        public override bool MatchQuery(World world, Entity entity)
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