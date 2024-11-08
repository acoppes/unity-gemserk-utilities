using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class LevelUpSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<LevelComponent>, Exc<DisabledComponent>> filter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in filter.Value)
            {
                ref var level = ref filter.Pools.Inc1.Get(entity);

                level.levelUpLastFrame = false;
                
                if (level.next >= level.max)
                {
                    level.next = level.max - 1;
                }
                
                if (level.next > level.current)
                {
                    // cant level up over max level
                    level.previous = level.current;
                    level.current = level.next;
                    
                    level.levelUpLastFrame = true;
                }
            }
        }
    }
}