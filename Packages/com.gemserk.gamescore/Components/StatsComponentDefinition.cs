using System;
using System.Collections.Generic;
using Gemserk.BitmaskTypes;
using Gemserk.Leopotam.Ecs;

namespace Game.Components
{
    public struct Stat
    {
        public const int Undefined = -1;
        
        public int type;
        public float baseValue;
        public float value;

        public bool hasStat => type != Undefined;

        public static Stat Default()
        {
            return new Stat()
            {
                type = Undefined,
                baseValue = 0,
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
                statsComponent.stats[i] = Stat.Default();
            }

            return statsComponent;
        }

        public void SetStat(Stat stat)
        {
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
    // 
    
     [Serializable]
    public class StatModifierDefinition
    {
        public IntTypeAsset statType;
        public float add = 0f;
        
        public float mult = 1f;

        public StatModifier CreateModifier()
        {
            return new StatModifier()
            {
                type = statType.value,
                add = add,
                mult = mult
            };
        }
    }
    
    [Serializable]
    public class StatsModifierDefinition
    {
        public IntTypeAsset modifierType;
        public float time;
        
        public List<StatModifierDefinition> statsModifiers;

        public StatsModifier CreateModifier()
        {
            var statsModifiersData = StatsModifier.Default();
            statsModifiersData.type = modifierType ? modifierType.value : StatsModifier.Undefined;
            statsModifiersData.time = time;

            foreach (var statModifierDefinition in statsModifiers)
            {
                statsModifiersData.modifiers[statModifierDefinition.statType.value] = statModifierDefinition.CreateModifier();
            }

            return statsModifiersData;
        }
    }
    
    public struct StatModifier
    {
        public const int Undefined = -1;
        
        public int type;
        public float add;
        public float mult;
    }
    
    public struct StatsModifier
    {
        public const int Undefined = -1;
        
        public enum State
        {
            Inactive,
            Refresh,
            Active
        }

        public int type;

        public State previousState;
        public State state;
        
        public StatModifier[] modifiers;
        
        public float time;
        public float currentTime;
        public float activeTime;

        public bool active => time > 0 && currentTime < time;

        public bool remove;

        public static StatsModifier Default()
        {
            var statsModifier = new StatsModifier
            {
                type = Undefined,
                state = State.Inactive,
                modifiers = new StatModifier[StatsComponent.MaxStats],
                currentTime = 0,
                time = 0,
                previousState = State.Inactive
            };

            for (var i = 0; i < statsModifier.modifiers.Length; i++)
            {
                statsModifier.modifiers[i] = new StatModifier()
                {
                    type = StatModifier.Undefined,
                    add = 0,
                    mult = 1
                };
            }

            return statsModifier;
        }

        public ref StatModifier Get(int statType)
        {
            return ref modifiers[statType];
        }
    }
    
    public struct StatsModifiersComponent : IEntityComponent
    {
        public static int MaxModifiers = 16;
        
        public StatsModifier[] statsModifiers;

        public bool pendingRecalculateStats;
        
        public static StatsModifiersComponent Default()
        {
            var statsModifiers = new StatsModifier[MaxModifiers];

            for (var i = 0; i < statsModifiers.Length; i++)
            {
                statsModifiers[i] = StatsModifier.Default();
            }

            return new StatsModifiersComponent()
            {
                statsModifiers = statsModifiers,
                pendingRecalculateStats = true
            };
        }

        public void Add(StatsModifier modifier)
        {
            // should I keep a separated list of pending modifiers to be added instead and process in system?
            
            // this will copy pointers to other arrays
            var currentModifier = modifier;
            
            currentModifier.state = StatsModifier.State.Refresh;
            
            statsModifiers[modifier.type] = currentModifier;
        }

        public void Remove(int modifierType)
        {
            statsModifiers[modifierType].remove = true;
        }
    }

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
                    value = statDefinition.value
                });
            }
            
            entity.Add(stats);
            entity.Add(StatsModifiersComponent.Default());
            
            // Should add automatically th estats modifier component too here?
            
            // entity.Add(new GenericStatComponent<Stat>());
        }
    }
}