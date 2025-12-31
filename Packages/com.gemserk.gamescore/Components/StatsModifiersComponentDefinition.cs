using System;
using System.Collections.Generic;
using Gemserk.BitmaskTypes;
using Gemserk.Leopotam.Ecs;

namespace ShipMiner.Components
{
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
            statsModifiersData.type = modifierType.value;
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
        public enum State
        {
            Inactive,
            Pending,
            Added,
            Active,
            Removed
        }

        public int type;

        public State previousState;
        public State state;
        
        public StatModifier[] modifiers;
        
        public float time;
        public float currentTime;
        public float activeTime;

        public bool active => time > 0 && currentTime < time;

        public static StatsModifier Default()
        {
            var statsModifier = new StatsModifier
            {
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
    }
    
    public struct StatsModifiersComponent : IEntityComponent
    {
        public static int MaxModifiers = 16;
        
        public StatsModifier[] statsModifiers;
        
        public static StatsModifiersComponent Default()
        {
            var statsModifiers = new StatsModifier[MaxModifiers];

            for (var i = 0; i < statsModifiers.Length; i++)
            {
                statsModifiers[i] = StatsModifier.Default();
            }

            return new StatsModifiersComponent()
            {
                statsModifiers = statsModifiers
            };
        }

        public void Add(StatsModifier modifier)
        {
            // should I keep a separated list of pending modifiers to be added instead and process in system?
            
            var currentModifier = statsModifiers[modifier.type];
            
            if (currentModifier.state is StatsModifier.State.Inactive or StatsModifier.State.Removed)
            {
                currentModifier = modifier;
                currentModifier.state = StatsModifier.State.Pending;
            }
            else
            {
                // refresh time here
                currentModifier.currentTime = 0;
            }
            statsModifiers[modifier.type] = currentModifier;
        }
    }
    
    public class StatsModifiersComponentDefinition : ComponentDefinitionBase
    {
        public override void Apply(World world, Entity entity)
        {
            entity.Add(StatsModifiersComponent.Default());
        }
    }
}