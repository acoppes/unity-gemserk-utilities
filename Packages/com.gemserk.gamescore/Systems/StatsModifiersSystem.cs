using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class StatsModifiersSystem : BaseSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<StatsModifiersComponent>, Exc<DisabledComponent>> modifiersFilter = default;
        private readonly EcsFilterInject<Inc<StatsComponent, StatsModifiersComponent>, Exc<DisabledComponent>> statsFilter = default;
        
        // private readonly EcsFilterInject<Inc<StatsModifiersComponent, ControllerComponent>, Exc<DisabledComponent>> controllersFilter = default;
        // on stats refresh we could invoke the callback
        
        public void Run(EcsSystems systems)
        {
            var deltaTime = dt;
            
            foreach (var e in modifiersFilter.Value)
            {
                ref var statsModifiers = ref modifiersFilter.Pools.Inc1.Get(e);
                statsModifiers.pendingRecalculateStats = false;
                
                for (var i = 0; i < statsModifiers.statsModifiers.Length; i++)
                {
                    var statsModifier = statsModifiers.statsModifiers[i];

                    if (statsModifier.type == StatsModifier.Undefined)
                    {
                        continue;
                    }
                    
                    // mark modifier as added or removed or similar to react later
                    
                    statsModifier.previousState = statsModifier.state;
                    
                    if (statsModifier.state == StatsModifier.State.Refresh)
                    {
                        statsModifier.currentTime = 0;
                        statsModifier.activeTime = 0;
                        statsModifier.state = StatsModifier.State.Active;
                        statsModifiers.pendingRecalculateStats = true;
                    }
                    
                    if (statsModifier.state == StatsModifier.State.Active)
                    {
                        statsModifier.activeTime += deltaTime;
                        statsModifier.currentTime += deltaTime;
                        
                        // if modifier just set not sure how
                        if (statsModifier.remove || (statsModifier.time > 0 && statsModifier.currentTime > statsModifier.time))
                        {
                            statsModifier.type = StatsModifier.Undefined;
                            
                            statsModifier.state = StatsModifier.State.Inactive;
                            statsModifiers.pendingRecalculateStats = true;

                            statsModifier.remove = false;
                        }
                    }

                    statsModifiers.statsModifiers[i] = statsModifier;
                }
            }
            
            foreach (var e in statsFilter.Value)
            {
                ref var stats = ref statsFilter.Pools.Inc1.Get(e);
                ref var statsModifiers = ref statsFilter.Pools.Inc2.Get(e);

                if (statsModifiers.pendingRecalculateStats)
                {
                    for (var i = 0; i < stats.stats.Length; i++)
                    {
                        var stat = stats.stats[i];
                        // stat.value = stat.baseValue;
                        
                        stat.add = 0f;
                        stat.mult = 1f;
                        
                        stats.stats[i] = stat;
                    }

                    foreach (var statsModifier in statsModifiers.statsModifiers)
                    {
                        if (statsModifier.type == StatsModifier.Undefined)
                        {
                            continue;
                        }
                        
                        if (statsModifier.state != StatsModifier.State.Active)
                        {
                            continue;
                        }
                        
                        // first all additions
                        foreach (var modifier in statsModifier.modifiers)
                        {
                            var stat = stats.GetStat(modifier.type);
                            // stat.value += modifier.add;
                            stat.add += modifier.add;
                            stat.mult *= modifier.mult;
                            stats.SetStat(stat);
                        }

                        // then all multiplications
                        // foreach (var modifier in statsModifier.modifiers)
                        // {
                        //     var stat = stats.GetStat(modifier.type);
                        //     stat.value *= modifier.mult;
                        //     stats.SetStat(stat);
                        // }
                        
                        // so the value is (baseValue + add + add + add) * mult * mult * mult
                    }

                    statsModifiers.pendingRecalculateStats = false;
                }
            }
            
            // TODO: send event to controllers when a modifier changed its state
            
            // foreach (var e in statsChanged.Value)
            // {
            //     statsChanged.Pools.Inc1.Del(e);
            // }
        }
    }
}