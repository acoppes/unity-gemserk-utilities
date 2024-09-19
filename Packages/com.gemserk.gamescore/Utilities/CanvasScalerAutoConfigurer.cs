using UnityEngine;
using UnityEngine.UI;

namespace Game.Utilities
{
    [ExecuteAlways]
    public class CanvasScalerAutoConfigurer : MonoBehaviour
    {
        private CanvasScaler canvasScaler;

        public float scaleOffset = 0;
        public float multiplier = 1;
        
        public float referenceWidth;
        // public float referenceHeight;
        public float maxScaleFactor;

        public bool executeInEditMode;
        
        private bool wasFullscreen;
        
        private Resolution lastResolution;

        
        private void Awake()
        {
            if (!canvasScaler)
            {
                canvasScaler = GetComponent<CanvasScaler>();
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
            var newScaleFactor = Mathf.Round(Screen.width / referenceWidth);
            newScaleFactor = Mathf.Clamp(newScaleFactor, 1, maxScaleFactor);
            canvasScaler.scaleFactor = (newScaleFactor + scaleOffset) * multiplier;
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