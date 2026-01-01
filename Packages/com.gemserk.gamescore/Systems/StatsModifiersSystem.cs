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

                    if (statsModifier.type == StatModifier.Undefined)
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
                        // if modifier just set not sure how
                        if (statsModifier.remove || (statsModifier.time > 0 && statsModifier.currentTime > statsModifier.time))
                        {
                            statsModifier.state = StatsModifier.State.Inactive;
                            statsModifiers.pendingRecalculateStats = true;
                        }
                    }
                    
                    // TODO: if modifier values (add and mult) changed (can be calculated in each modifier set),
                    // then mark stats modifier to refresh too. 
                    
                    statsModifier.activeTime += deltaTime;
                    statsModifier.currentTime += deltaTime;

                    statsModifiers.statsModifiers[i] = statsModifier;
                }
            }
            
            foreach (var e in statsFilter.Value)
            {
                ref var stats = ref statsFilter.Pools.Inc1.Get(e);
                ref var statsModifiers = ref statsFilter.Pools.Inc2.Get(e);
                
                // if statsMOdifiers has a change then
                // reset stats to original values
                // recalculate stat from all modifiers, not only added/removed

                if (statsModifiers.pendingRecalculateStats)
                {
                    for (var i = 0; i < stats.stats.Length; i++)
                    {
                        var stat = stats.stats[i];
                        stat.value = stat.baseValue;
                        stats.stats[i] = stat;
                    }

                    foreach (var statsModifier in statsModifiers.statsModifiers)
                    {
                        if (statsModifier.type == StatModifier.Undefined)
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
                            if (modifier.type == StatModifier.Undefined)
                                continue;
                                
                            var stat = stats.GetStat(modifier.type);
                            stat.value += modifier.add;
                            stats.SetStat(stat);
                        }

                        // then all multiplications
                        foreach (var modifier in statsModifier.modifiers)
                        {
                            if (modifier.type == StatModifier.Undefined)
                                continue;
                                
                            var stat = stats.GetStat(modifier.type);
                            stat.value *= modifier.mult;
                            stats.SetStat(stat);
                        }
                        
                        // so the value is (baseValue + add + add + add) * mult * mult * mult
                    }
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