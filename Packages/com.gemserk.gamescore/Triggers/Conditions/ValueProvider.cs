using System;
using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;
using Gemserk.Utilities;
using MyBox;
using Object = UnityEngine.Object;

namespace Game.Triggers.Conditions
{
    [Serializable]
    public class ValueProvider : IValueProvider
    {
        public enum SourceType
        {
            Constant = 0,
            Count = 1,
            Delegate = 2
        }

        public SourceType sourceType;

        [ConditionalField(nameof(sourceType), false,  SourceType.Constant)]
        public float constantValue;

        [ConditionalField(nameof(sourceType), false,  SourceType.Count)]
        public TriggerTarget target;
        
        [ConditionalField(nameof(sourceType), false,  SourceType.Delegate)]
        [ObjectType(typeof(IValueProvider))]
        public Object delegateProvider;
        
        public int GetIntValue(World world, object activator)
        {
            if (sourceType == SourceType.Constant)
            {
                return (int)constantValue;
            }
            
            if (sourceType == SourceType.Count)
            {
                var results = new List<Entity>();
                target.Get(results, world, activator);
                return results.Count;
            }
            
            if (sourceType == SourceType.Delegate)
            {
                return delegateProvider.GetInterface<IValueProvider>().GetIntValue(world, activator);
            }
            
            return 0;
        }

        public override string ToString()
        {
            if (sourceType == SourceType.Constant)
            {
                return $"{constantValue:0.0}";
            }
                    
            if (sourceType == SourceType.Count)
            {
                return target.ToString();
            }

            if (sourceType == SourceType.Delegate)
            {
                return delegateProvider ? delegateProvider.name : string.Empty;
            }
            
            return string.Empty;
        }
    }
}