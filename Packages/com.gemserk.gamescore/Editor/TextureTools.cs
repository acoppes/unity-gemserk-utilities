using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

namespace Game.Editor
{
    public static class TextureTools
    {
        [UnityEditor.MenuItem("Assets/Texture Tools/Extract LUT2D from Texture")]
        public static void Create()
        {
            if (Selection.activeObject is not Texture2D)
            {
                EditorUtility.DisplayDialog("Error", "Select texture", "ok");
                return;
            }

            var targetFilePath = EditorUtility.SaveFilePanel("Select target file", "", "generatedLut", "png");

            if (string.IsNullOrEmpty(targetFilePath))
            {
                return;
            }
            
            var texture = Selection.activeObject as Texture2D;
            var temporaryTexture = new Texture2D(texture.width, texture.height, texture.format, false)
            {
                filterMode = texture.filterMode,
                wrapMode = texture.wrapMode
            };

            try
            {
                Graphics.CopyTexture(texture, temporaryTexture);

                var lutSize = new Vector2Int(32, 32);

                // var colors = temporaryTexture.GetPixels();
                var lutTexture = new Texture2D(lutSize.x, lutSize.y, texture.format, false);
                // var lutPixels = lutTexture.GetPixels();
                
                for (var i = 0; i < lutTexture.width; i++)
                {
                    for (var j = 0; j < lutTexture.height; j++)
                    {
                        lutTexture.SetPixel(i, j, new Color(1, 0, 1, 0));
                    }
                }

                for (var i = 0; i < temporaryTexture.width; i++)
                {
                    for (var j = 0; j < temporaryTexture.height; j++)
                    {
                        var color = temporaryTexture.GetPixel(i, j);
                        
                        // var color = colors[i + j * temporaryTexture.width];
                
                        var x = Mathf.FloorToInt(color.r * lutTexture.width);
                        var y = Mathf.FloorToInt(color.g * lutTexture.height);

                        color.a = 1;
                        
                        lutTexture.SetPixel(x, y, color);
                    }
                }
                
                // lutTexture.SetPixels(lutPixels);
                lutTexture.Apply();

                var pngBytes = lutTexture.EncodeToPNG();
                File.WriteAllBytes(targetFilePath, pngBytes);
                AssetDatabase.Refresh();
            }
            finally
            {
                Object.DestroyImmediate(temporaryTexture);
            }

        }
    }
}