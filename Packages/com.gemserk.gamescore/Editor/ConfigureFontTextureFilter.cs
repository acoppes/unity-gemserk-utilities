using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public static class ConfigureFontTextureFilter
    {
        [MenuItem("Gemserk/Fonts/Font Texture Filter")]
        public static void ConfigureSelectedFontTextureFilter()
        {
            var font = Selection.activeObject as Font;
            if (font)
            {
                font.material.mainTexture.filterMode = FilterMode.Point;
            }
        }
    }
}