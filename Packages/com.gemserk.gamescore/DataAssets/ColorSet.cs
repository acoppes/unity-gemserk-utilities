using System;
using UnityEngine;

namespace Game.DataAssets
{
    [CreateAssetMenu(menuName="Gemserk/ColorSet")]
    public class ColorSet : ScriptableObject
    {
        public Color[] colors;
    }

    public static class ColorSetExtensions
    {
        public static Texture2D CreateLutTexture(this ColorSet colorSet)
        {
            var lutColors = new Color[colorSet.colors.Length];
                    
            Array.Copy(colorSet.colors, lutColors, lutColors.Length);
                    
            var lutTexture = new Texture2D(lutColors.Length, 1, TextureFormat.ARGB32, false);
            lutTexture.filterMode = FilterMode.Point;
            lutTexture.wrapMode = TextureWrapMode.Clamp;
            lutTexture.SetPixels(lutColors);
            lutTexture.Apply();
            
            return lutTexture;
        }
    }
}