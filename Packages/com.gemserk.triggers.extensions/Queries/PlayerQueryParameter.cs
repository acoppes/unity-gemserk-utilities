using Gemserk.Leopotam.Ecs;

namespace Gemserk.Triggers.Queries
{
    public struct PlayerParameter : IEntityMatcher
    {
        public int player;

        public PlayerParameter(int player)
        {
            this.player = player;
        }
        
        public bool Match(Entity entity)
        {
            if (!entity.Has<PlayerComponent>())
            {
                return false;
            }
            
            return entity.Get<PlayerComponent>().player == player;
        }
    }
    
    public class PlayerQueryParameter : EntityMatcherBase
    {
        public int player;
        
        public override bool Match(Entity entity)
        {
            return new PlayerParameter(player).Match(entity);
        }

        public override string ToString()
        {
            return $"player:{player}";
        }
    }
}