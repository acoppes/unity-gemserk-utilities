using MyBox;
using UnityEngine;

namespace Game.Utilities
{
    public class CanvasSizeAutoConfigurer : MonoBehaviour
    {
        // TODO: the idea is to scale the canvas to maximize in the world scale reference
        
        public RectTransform canvasTransform;

        public float ppu = 16f;
        public float referenceWidth;

        public bool executeInEditMode;
        
        private bool wasFullscreen;
        
        private Resolution lastResolution;
        
        private void Awake()
        {
            if (!canvasTransform)
            {
                canvasTransform = GetComponent<RectTransform>();
            }
        }

        private void Start()
        {
            if (!Application.isPlaying && !executeInEditMode)
                return;
            
            FixScaleFactor();
        }

        private void FixScaleFactor()
        {
            var scaleFactor = referenceWidth / Screen.width;
            var fixedHeight = Screen.height * scaleFactor; 
            
            canvasTransform.SetWidth(referenceWidth / ppu);
            canvasTransform.SetHeight(fixedHeight / ppu);
            
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