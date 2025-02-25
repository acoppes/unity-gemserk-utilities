using UnityEngine;

namespace Game.Utilities
{
    public abstract class BaseWeightFunction : MonoBehaviour
    {
        public AnimationCurve weightCurve = AnimationCurve.Linear(0, 0, 1, 1);
        
        public abstract float Compare(Target x, RuntimeTargetingParameters targetingParameters);
    }
}