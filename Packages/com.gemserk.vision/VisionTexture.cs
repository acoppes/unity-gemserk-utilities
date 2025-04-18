﻿using System;
using Gemserk.DataGrids;
using UnityEngine;

namespace Gemserk.Vision
{
    public class VisionTexture : MonoBehaviour
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
        protected Color _greyColor = new Color(0.5f, 0, 0, 1.0f);
    
        [SerializeField]
        protected Color _whiteColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        [SerializeField] protected Color _startColor = new Color(0, 0, 0, 1.0f);

        [SerializeField]
        protected float _interpolateColorSpeed;

        private const float _defaultInterpolationColorSpeed = 6.0f;

        [SerializeField]
        protected bool _previousVision = true;

        public bool PreviousVision
        {
            get { return _previousVision; }
            set { _previousVision = value; }
        }

        public bool ColorInterpolation
        {
            get { return _interpolateColorSpeed > 0; }
            set { _interpolateColorSpeed = value ? _defaultInterpolationColorSpeed  : 0.0f; }
        }
	
        public void Create(int width, int height, Vector2 scale)
        {
            _texture =  new Texture2D(width, height, _textureFormat, false, false);
            _texture.filterMode = FilterMode.Point;
            _texture.wrapMode = TextureWrapMode.Clamp;
		
            _spriteRenderer.sprite = Sprite.Create(_texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 1);
            _spriteRenderer.transform.localScale = scale;
        
            _colors = new Color[width * height];
        
            for (var i = 0; i < width * height; i++)
            {
                _colors[i] = _startColor;
            }
        
            _texture.SetPixels(_colors);
            _texture.Apply();
        }

        public void UpdateTexture(Grid<int> visionData, Grid<int> previousVisionData, int activePlayer)
        {
            var interpolationEnabled = _interpolateColorSpeed > Mathf.Epsilon;
            var alpha = Time.deltaTime * _interpolateColorSpeed;
        
            var width = visionData.width;
            var height = visionData.height;

            for (var i = 0; i < width * height; i++)
            {
                var newColor = _startColor;

                var isVisible = (visionData.ReadValue(i) & activePlayer) == activePlayer;
                var wasVisible = (previousVisionData.ReadValue(i) & activePlayer) == activePlayer;

                if (isVisible)
                {
                    newColor = _whiteColor;
                } else if (_previousVision && wasVisible)
                {
                    newColor = _greyColor;
                } 
        
                if (interpolationEnabled)
                {
                    newColor.r = Mathf.LerpUnclamped(_colors[i].r, newColor.r, alpha);
                }

                _colors[i] = newColor;
            }
        
            _texture.SetPixels(_colors);
            _texture.Apply();
        }

    }
}