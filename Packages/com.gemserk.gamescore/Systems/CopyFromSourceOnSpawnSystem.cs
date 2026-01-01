using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class CopyFromSourceOnSpawnSystem : BaseSystem, IEntityCreatedHandler
    {
        readonly EcsFilterInject<Inc<SourceEntityComponent, PlayerComponent>, Exc<DisabledComponent>> playerFilter = default;
        readonly EcsFilterInject<Inc<SourceEntityComponent, StatsModifiersComponent>, Exc<DisabledComponent>> modifiersFilter = default;
        
        public void OnEntityCreated(World world, Entity entity)
        {
            if (entity.Has<SourceEntityComponent>())
            {
                var source = entity.Get<SourceEntityComponent>();
                if (entity.Has<PlayerComponent>() && source.source.Has<PlayerComponent>())
                {
                    entity.Get<PlayerComponent>().player = source.source.Get<PlayerComponent>().player;
                }
                
                if (entity.Has<StatsModifiersComponent>() && source.source.Has<StatsModifiersComponent>())
                {
                    var modifiers = source.source.Get<StatsModifiersComponent>();

                    foreach (var modifier in modifiers.statsModifiers)
                    {
                        if (modifier.state != StatsModifier.State.Inactive)
                        {
                            entity.Get<StatsModifiersComponent>().Add(modifier);
                        }
                    }
                }
            }
        }
    }
}