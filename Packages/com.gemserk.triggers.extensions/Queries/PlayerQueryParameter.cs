using Gemserk.Leopotam.Ecs;

namespace Gemserk.Triggers.Queries
{
    public struct PlayerEntityQuery : IQueryParameter
    {
        public int player;

        public PlayerEntityQuery(int player)
        {
            this.player = player;
        }
        
        public bool MatchQuery(World world, Entity entity)
        {
            if (!entity.Has<PlayerComponent>())
            {
                return false;
            }
            
            return entity.Get<PlayerComponent>().player == player;
        }
    }
    
    public class PlayerQueryParameter : QueryParameterBase
    {
        public int player;
        
        public override bool MatchQuery(World world, Entity entity)
        {
            return new PlayerEntityQuery(player).MatchQuery(world, entity);
        }

        public override string ToString()
        {
            return $"player:{player}";
        }
    }
}