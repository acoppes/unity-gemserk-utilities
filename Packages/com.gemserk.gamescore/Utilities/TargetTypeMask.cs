using Gemserk.BitmaskTypes;
using UnityEngine;

namespace Game.Utilities
{
    public interface ITargetTypeMask
    {
        int GetTargetTypeMask();
    }
    
    public class TargetTypeMask : MonoBehaviour, ITargetTypeMask
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