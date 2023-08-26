using System.Collections.Generic;
using Gemserk.BitmaskTypes;

namespace MyGame
{
    public static class GenericTypeAssetExtensions
    {
        public static string GetTypeName(this GenericTypeCategoryAsset asset, int value)
        {
            foreach (var type in asset.types)
            {
                if (type == null)
                    continue;

                if (type.type == value)
                    return type.name;
            }

            return null;
        }
        
        public static void GetMaskNames(this GenericTypeCategoryAsset asset, int mask, ICollection<string> names)
        {
            foreach (var type in asset.types)
            {
                if (type == null)
                    continue;

                if ((mask & type.type) == type.type) 
                    names.Add(type.name);
            }
        }
    }
}