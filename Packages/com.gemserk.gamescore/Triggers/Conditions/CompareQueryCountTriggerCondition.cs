﻿using Gemserk.Triggers;
using Gemserk.Triggers.Queries;
using MyBox;

namespace Game.Triggers.Conditions
{
    public class CompareQueryCountTriggerCondition : WorldTriggerCondition
    {
        public enum CompareType
        {
            EqualsTo = 0,
            GreaterThan = 1,
            LessThan = 2
        }

        public CompareType compareType;
        
        public int count;
        
        [DisplayInspector]
        public Query query;
        
        public override string GetObjectName()
        {
            if (query == null)
            {
                return "Count()";
            }
            return $"{compareType}({count}, {query})";
        }
        
        public override bool Evaluate(object activator = null)
        {
            if (query == null || world == null)
            {
                return false;
            }

            var entities = world.GetEntities(query.GetEntityQuery());

            return compareType switch
            {
                CompareType.EqualsTo => entities.Count == count,
                CompareType.GreaterThan => entities.Count > count,
                CompareType.LessThan => entities.Count < count,
                _ => false
            };
        }
    }
}