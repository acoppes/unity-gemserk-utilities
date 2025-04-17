using UnityEngine;

namespace Gemserk.DataGrids
{
    public class GridMaskDataTexture
    {
        private readonly Texture2D _texture;

        private readonly Color[] _colors;
    
        private readonly Color _startColor = new Color(0, 0, 0, 1.0f);
    
        private readonly Color[] _validColors;
    
        public GridMaskDataTexture(TextureFormat textureFormat, SpriteRenderer spriteRenderer, Color[] validColors, 
            int width, int height, float gridWidth, float gridHeight)
        {
            _validColors = validColors;
            
            _texture =  new Texture2D(width, height, textureFormat, false, false);
            _texture.filterMode = FilterMode.Point;
            _texture.wrapMode = TextureWrapMode.Clamp;
		
            spriteRenderer.sprite = Sprite.Create(_texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 1);
            spriteRenderer.transform.localScale = new Vector3(gridWidth, gridHeight, 1.0f);
        
            _colors = new Color[width * height];
        
            for (var i = 0; i < width * height; i++)
            {
                _colors[i] = _startColor;
            }
        
            _texture.SetPixels(_colors);
            _texture.Apply();
        }

        private Grid<int> _worldMatrix;
        
        public void UpdateTexture(Grid<int> worldMatrix)
        {
            _worldMatrix = worldMatrix;
         
            var width = _worldMatrix.width;
            var height = _worldMatrix.height;

            for (var i = 0; i < width * height; i++)
            {
                var newColor = _startColor;

                var data = _worldMatrix.ReadValue(i);

                for (var j = 0; j < _validColors.Length; j++)
                {
                    var color = _validColors[j];

                    if ((data & 1 << j) == 0)
                        continue;

                    newColor += color;
                }
    
                _colors[i] = newColor;
            }
        
            _texture.SetPixels(_colors);
            _texture.Apply();
        }

    }
}