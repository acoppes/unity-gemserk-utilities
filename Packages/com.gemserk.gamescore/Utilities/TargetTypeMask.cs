using Gemserk.BitmaskTypes;
using UnityEngine;

namespace Game.Utilities
{
    public class TargetTypeMask : MonoBehaviour, ITargetTypeMask
    {
        public bool everything;
        
        [Tooltip("This is ignored if everything bool is turned on")]
        public IntTypeAsset[] targetTypes;

        private int mask;

        public int GetTargetTypeMask()
        {
            if (everything)
            {
                return -1;
            }
            
            mask = 0;

            for (var i = 0; i < targetTypes.Length; i++)
            {
                mask |= targetTypes[i].value;
            }

            return mask;
        }
    }
}