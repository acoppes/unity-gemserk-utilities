using Gemserk.BitmaskTypes;
using UnityEngine;

namespace Game.DataAssets
{
    [CreateAssetMenu(menuName = "Unit Type")]
    public class UnitTypeAsset : BitmaskTypeAsset
    {
        public bool MatchType(int otherType)
        {
            return BitMaskCheck.Match(type, otherType);
            // return type.HasUnitType(otherType);
        }
    }
}