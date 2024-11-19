using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class XpLevelUpSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<XpComponent, LevelComponent>, Exc<DisabledComponent>> filter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in filter.Value)
            {
                ref var xp = ref filter.Pools.Inc1.Get(entity);
                ref var level = ref filter.Pools.Inc2.Get(entity);
                
                xp.total = xp.baseTotal + xp.totalIncrementPerLevel * level.current;

                xp.current += xp.pending;
                xp.pending = 0;
                
                if (xp.current > xp.total)
                {
                    // cap
                    xp.current = xp.total;
                    if (!level.IsMaxLevel)
                    {
                        level.QueueLevelUp();
                        xp.current = 0;
                    }
                }
            }
        }
    }
}