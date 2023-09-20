using System;
using Game.Components;
using MyBox;
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

        public CheckDistanceType distanceType;
        public MinMaxFloat range;

        public PlayerAllianceType playerAllianceType;

        public HealthComponent.AliveType aliveType;

        public MinMaxFloat angle;

        public float maxRangeSqr => range.Max * range.Max;
        public float minRangeSqr => range.Min * range.Min;

        public Object sorter;
    }
}