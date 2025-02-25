using UnityEngine;

namespace Game.Utilities
{
    public class DistanceSqrWeightFunction : BaseWeightFunction
    {
        public override float Compare(Target x, RuntimeTargetingParameters runtimeTargetingParameters)
        {
            var diff = x.position - runtimeTargetingParameters.position;
            var distanceSqr = diff.sqrMagnitude;
            
            var distanceFactor = Mathf.InverseLerp(runtimeTargetingParameters.filter.minRangeSqr,
                runtimeTargetingParameters.filter.maxRangeSqr, distanceSqr);

            return weightCurve.Evaluate(distanceFactor);
        }
    }
}