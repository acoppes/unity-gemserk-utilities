using System;
using Gemserk.DataGrids;
using UnityEngine;

namespace Gemserk.Vision.Debug
{
    public class VisionTerrainTexture : MonoBehaviour
    {
        [SerializeField]
        protected TextureFormat _textureFormat;

        [SerializeField]
        protected SpriteRenderer _spriteRenderer;

        [NonSerialized]
        private Texture2D _texture;

        [NonSerialized]
        private Color[] _colors;

        [SerializeField] 
        protected Color[] _terrainColors = new Color[]
        {
            new Color(0, 0, 0.2f, 1.0f),
            new Color(0, 0, 0.6f, 1.0f),
            new Color(0, 0, 0.8f, 1.0f),
        };

        public void Create(int width, int height, Vector2 scale)
        {
            _texture =  new Texture2D(width, height, _textureFormat, false, false);
            _texture.filterMode = FilterMode.Point;
            _texture.wrapMode = TextureWrapMode.Clamp;
		
            _spriteRenderer.sprite = Sprite.Create(_texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 1);
            _spriteRenderer.transform.localScale = scale;
        
            _colors = new Color[width * height];
        
            _texture.SetPixels(_colors);
            _texture.Apply();
        }

        public void UpdateTexture(Grid<int> data)
        {
            if (data.values == null)
                return;

            var width = data.width;
            var height = data.height;
        
            for (var i = 0; i < width * height; i++)
            {
                var ground = data.ReadValue(i);
                var terrainColor = Color.black;
                if (ground < _terrainColors.Length)
                    terrainColor = _terrainColors[ground];
                _colors[i] = terrainColor;
            }
		
            _texture.SetPixels(_colors);
            _texture.Apply();
        }
    }
}