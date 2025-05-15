using System;
using Game.Components;
using MyBox;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Utilities
{
    [Serializable]
    public struct TargetingFilter
    {
        public enum CheckDistanceType
        {
            Nothing = 0,
            InsideDistance = 1,
            InsideDistanceXZ = 2,
        }

        public TargetType targetTypes;
        public Object targetTypeMask;

        // public int TargetTypes => targetTypeMask ? targetTypeMask.GetTargetTypeMask() : (int) targetTypes;
        
        public CheckDistanceType distanceType;
        public MinMaxFloat range;

        public PlayerAllianceType playerAllianceType;

        public HealthComponent.AliveType aliveType;

        [Tooltip("Use 0 to disable, max shouldn't be bigger than 180")]
        public MinMaxFloat angle;

        public float maxRangeSqr => range.Max * range.Max;
        public float minRangeSqr => range.Min * range.Min;

        public Object sorter;
        public Object customFilter;
    }
}