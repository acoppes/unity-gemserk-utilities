using System;
using System.Collections.Generic;
using Gemserk.BitmaskTypes;
using Gemserk.Leopotam.Ecs;

namespace Game.Components
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
    
    public class StatsModifiersComponentDefinition : ComponentDefinitionBase
    {
        public override void Apply(World world, Entity entity)
        {
            entity.Add(StatsModifiersComponent.Default());
        }
    }
}