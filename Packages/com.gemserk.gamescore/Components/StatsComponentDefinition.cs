using System;
using System.Collections.Generic;
using Gemserk.BitmaskTypes;
using Gemserk.Leopotam.Ecs;

namespace ShipMiner.Components
{
    public struct Stat
    {
        public int setCount;
        
        public int type;
        public float baseValue;

        public float add;
        public float mult;

        public float value => (baseValue + add) * mult;

        public bool isSet => setCount > 0;

        public static Stat Default(int type)
        {
            return new Stat()
            {
                type = type,
                baseValue = 0,
                add = 0,
                mult = 1
            };
        }
    }

    // public struct StatsChangedComponent : IEventComponent
    // {
    //     
    // }
    
    public struct StatsComponent : IEntityComponent
    {
        public static int MaxStats = 16;
        
        public Stat[] stats;

        public static StatsComponent Default()
        {
            var statsComponent = new StatsComponent()
            {
                stats = new Stat[MaxStats]
            };
            for (var i = 0; i < statsComponent.stats.Length; i++)
            {
                statsComponent.stats[i] = Stat.Default(i);
            }

            return statsComponent;
        }

        public void SetStat(Stat stat)
        {
            // mark modified?
            var currentStat = stats[stat.type];
            stat.setCount = currentStat.setCount + 1;
            stats[stat.type] = stat;
        }

        public Stat GetStat(int type)
        {
            return stats[type];
        }
    }

    // public struct GenericStatComponent<T> : IEntityComponent where T : struct
    // {
    //     public T value;
    //     
    //     public float add;
    //     public float mult;
    // }

    public class StatsComponentDefinition : ComponentDefinitionBase
    {
        [Serializable]
        public class StatDefinition
        {
            // public int type;
            public IntTypeAsset typeAsset;
            public float value;
        }

        public List<StatDefinition> statDefinitions;
        
        public override void Apply(World world, Entity entity)
        {
            var stats = StatsComponent.Default();
            foreach (var statDefinition in statDefinitions)
            {
                stats.SetStat(new Stat()
                {
                    type = statDefinition.typeAsset.value,
                    baseValue = statDefinition.value,
                    add = 0,
                    mult = 1,
                });
            }
            entity.Add(stats);
            
            // entity.Add(new GenericStatComponent<Stat>());
        }
    }
}