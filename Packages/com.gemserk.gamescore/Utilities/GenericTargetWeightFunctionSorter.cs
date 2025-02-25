using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utilities
{
    public class GenericTargetWeightFunctionSorter : MonoBehaviour, ITargetSorter
    {
        [Serializable]
        public class InternalTargetComparer : Comparer<Target>
        {
            [NonSerialized]
            public RuntimeTargetingParameters runtimeTargetingParameters;

            public List<BaseWeightFunction> weightFunctions;

            public bool normalized;
            
            public override int Compare(Target x, Target y)
            {
                var xWeight = 0f;
                var yWeight = 0f;
                
                foreach (var weightFunction in weightFunctions)
                {
                    xWeight += weightFunction.Compare(x, runtimeTargetingParameters);
                    yWeight += weightFunction.Compare(y, runtimeTargetingParameters);
                }

                if (normalized && weightFunctions.Count > 0)
                {
                    xWeight /= weightFunctions.Count;
                    yWeight /= weightFunctions.Count;
                }
                
                if (xWeight > yWeight)
                {
                    return -1;
                }
                
                if (xWeight < yWeight)
                {
                    return 1;
                }
                
                return 0;
            }
        }

        public InternalTargetComparer targetComparer;

        public void Sort(List<Target> targets, RuntimeTargetingParameters runtimeTargetingParameters)
        {
            targetComparer.runtimeTargetingParameters = runtimeTargetingParameters;
            targets.Sort(targetComparer);
        }
    }
}