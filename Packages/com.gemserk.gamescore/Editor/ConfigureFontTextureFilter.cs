using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public static class ConfigureFontTextureFilter
    {
        [MenuItem("GBJAM/Configure Selected Font Texture Filter")]
        public static void ConfigureSelectedFontTextureFilter()
        {
            var font = Selection.activeObject as Font;
            if (font != null)
            {
                font.material.mainTexture.filterMode = FilterMode.Point;
            }
        }
    }
}