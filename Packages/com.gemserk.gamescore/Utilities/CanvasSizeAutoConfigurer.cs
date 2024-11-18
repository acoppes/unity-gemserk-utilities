using System;
using MyBox;
using UnityEngine;

namespace Game.Utilities
{
    public class CanvasSizeAutoConfigurer : MonoBehaviour
    {
        // TODO: the idea is to scale the canvas to maximize in the world scale reference
        
        public RectTransform canvasTransform;
        public RectTransform targetRectTransform;

        public float ppu = 16f;

        // public float referenceWidth;

        public bool executeInEditMode;
        
        private bool wasFullscreen;
        
        private Resolution lastResolution;

        private void Start()
        {
            if (!Application.isPlaying && !executeInEditMode)
                return;
            
            FixScaleFactor();
        }

        private void FixScaleFactor()
        {
            var scaleFactorUsingWidthAsReference = Mathf.Round(Screen.width / canvasTransform.rect.width);
            // var heightScaleFactor = Mathf.Round(Screen.height / canvasTransform.rect.height);
            
            // var fixedHeight = Screen.height * scaleFactor; 

            var newWidth = Screen.width / scaleFactorUsingWidthAsReference * ppu;
            var newHeight = Screen.height / scaleFactorUsingWidthAsReference * ppu;

            newWidth = Mathf.Min(newWidth, canvasTransform.rect.width);
            newHeight = Mathf.Min(newHeight, canvasTransform.rect.height);
            
            targetRectTransform.SetWidth(newWidth);
            targetRectTransform.SetHeight(newHeight);
            
            // targetRectTransform.SetHeight(canvasTransform.rect.height / ppu);
            
            // var newScaleFactor = Mathf.Round(Screen.width / referenceWidth);
            // newScaleFactor = Mathf.Clamp(newScaleFactor, 1, maxScaleFactor);
            // canvasScaler.scaleFactor = (newScaleFactor + scaleOffset) * multiplier;
            
            wasFullscreen = Screen.fullScreen;
            lastResolution = Screen.currentResolution;
        }

        private void Update()
        {
            if (!Application.isPlaying && !executeInEditMode)
                return;
            
            var currentResolution = Screen.currentResolution;
            if (lastResolution.width != currentResolution.height || lastResolution.width != currentResolution.width || wasFullscreen != Screen.fullScreen)
            {
                FixScaleFactor();
            }
        }
    }
}