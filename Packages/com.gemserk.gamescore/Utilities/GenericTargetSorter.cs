using System;
using System.Collections.Generic;
using Gemserk.Utilities;
using UnityEngine;

namespace Game.Utilities
{
    public class GenericTargetSorter : MonoBehaviour, ITargetSorter
    {
        [Serializable]
        public class GenericTargetComparer : Comparer<Target>
        {
            [NonSerialized]
            public RuntimeTargetingParameters runtimeTargetingParameters;

            public AnimationCurve distanceWeight = AnimationCurve.Linear(0, 0, 1, 1);
            public AnimationCurve angleWeight = AnimationCurve.Linear(0, 0, 1, 1);
            public AnimationCurve healthWeight = AnimationCurve.Linear(0, 0, 1, 1);

            public override int Compare(Target x, Target y)
            {
                var aDifference = x.position - runtimeTargetingParameters.position;
                var bDifference = y.position - runtimeTargetingParameters.position;
                
                var xAngle = Vector2.Angle(runtimeTargetingParameters.direction, aDifference.normalized.XZ());
                var yAngle = Vector2.Angle(runtimeTargetingParameters.direction, bDifference.normalized.XZ());

                var xAngleFactor = Mathf.InverseLerp(runtimeTargetingParameters.filter.angle.Min,
                    runtimeTargetingParameters.filter.angle.Max, xAngle);
                var yAngleFactor = Mathf.InverseLerp(runtimeTargetingParameters.filter.angle.Min,
                    runtimeTargetingParameters.filter.angle.Max, yAngle);

                var xDistance = Mathf.InverseLerp(runtimeTargetingParameters.filter.minRangeSqr,
                    runtimeTargetingParameters.filter.maxRangeSqr, aDifference.sqrMagnitude);
                var yDistance = Mathf.InverseLerp(runtimeTargetingParameters.filter.minRangeSqr,
                    runtimeTargetingParameters.filter.maxRangeSqr, bDifference.sqrMagnitude);

                var xWeight = angleWeight.Evaluate(xAngleFactor) + distanceWeight.Evaluate(xDistance) + healthWeight.Evaluate(x.healthFactor);
                var yWeight = angleWeight.Evaluate(yAngleFactor) + distanceWeight.Evaluate(yDistance) + healthWeight.Evaluate(y.healthFactor);
            
                if (xWeight < yWeight)
                {
                    return -1;
                }

                if (xWeight > yWeight)
                {
                    return 1;
                }

                return 0;
            }
        }

        public GenericTargetComparer targetComparer;

        public void Sort(List<Target> targets, RuntimeTargetingParameters runtimeTargetingParameters)
        {
            targetComparer.runtimeTargetingParameters = runtimeTargetingParameters;
            targets.Sort(targetComparer);
        }
    }
}