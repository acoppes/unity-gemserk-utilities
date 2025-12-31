using UnityEngine;

namespace Game.Utilities
{
    public struct RuntimeTargetingParameters
    {
        // searcher properties
        public int alliedPlayersBitmask;

        public Vector3 position;
        public Vector3 direction;
            
        // filter properties
        public TargetingFilter filter;

        public float rangeMultiplier;
        
        public float rangeMultiplierSqr => rangeMultiplier * rangeMultiplier;
    }
}