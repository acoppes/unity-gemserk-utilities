using Gemserk.Leopotam.Ecs;

namespace Gemserk.Triggers.Queries
{
    public struct PlayerParameter : IQueryParameter
    {
        public int player;

        public PlayerParameter(int player)
        {
            this.player = player;
        }
        
        public bool MatchQuery(Entity entity)
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
        
        public override bool MatchQuery(Entity entity)
        {
            return new PlayerParameter(player).MatchQuery(entity);
        }

        public override string ToString()
        {
            return $"player:{player}";
        }
    }
}