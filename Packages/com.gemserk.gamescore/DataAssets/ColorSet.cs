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
            return CreateLutTexture(colorSet.colors);
        }
        
        public static Texture2D CreateLutTexture(Color[] colors)
        {
            var lutColors = new Color[colors.Length];
                    
            Array.Copy(colors, lutColors, lutColors.Length);
                    
            var lutTexture = new Texture2D(lutColors.Length, 1, TextureFormat.ARGB32, false)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };
            
            lutTexture.SetPixels(lutColors);
            lutTexture.Apply();
            
            return lutTexture;
        }
    }
}