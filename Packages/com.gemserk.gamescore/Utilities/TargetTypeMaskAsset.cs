using Gemserk.BitmaskTypes;
using UnityEngine;

namespace Game.Utilities
{
    [CreateAssetMenu(menuName = "Gemserk/TargetTypeMaskAsset", fileName = "TargetTypeMaskAsset", order = 0)]
    public class TargetTypeMaskAsset : ScriptableObject, ITargetTypeMask
    {
        public IntTypeAsset[] targetTypes;

        private int mask;
        private bool cached;

        public int GetTargetTypeMask()
        {
            if (!cached)
            {
                mask = 0;

                for (var i = 0; i < targetTypes.Length; i++)
                {
                    mask |= targetTypes[i].value;
                }
                cached = true;
            }

            return mask;
        }
    }
}