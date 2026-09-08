using UnityEngine;

namespace Game.Utilities
{
    public class DistanceSqrWeightFunction : BaseWeightFunction
    {
        public override float Evaluate(Target x, RuntimeTargetingParameters runtimeTargetingParameters)
        {
            var diff = x.position - runtimeTargetingParameters.position;
            var distanceSqr = diff.sqrMagnitude;

            var minRange = runtimeTargetingParameters.filter.range.Min - x.size;
            var maxRange = runtimeTargetingParameters.filter.range.Max + x.size;
            
            var distanceFactor = Mathf.InverseLerp(minRange * minRange, maxRange * maxRange, distanceSqr);

            return weightCurve.Evaluate(distanceFactor);
        }
    }
}