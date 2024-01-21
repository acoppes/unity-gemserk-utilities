using Unity.Cinemachine;
using Unity.Collections;
using UnityEngine;

namespace Game.Utilities
{
    [ExecuteInEditMode]
    public class FixedCameraSize : MonoBehaviour
    {
        [SerializeField]
        private CinemachineVirtualCamera virtualCamera;

        public bool updateWhileEditing = false;
        public bool updateWhilePlaying = false;
        
        public float heightUnits = 1;
        public float widthUnits = 1;

        [Range(0, 1)]
        public float match;

        [ReadOnly]
        public float result;

        public void Start()
        {
            if (!Application.isPlaying && !updateWhileEditing)
            {
                return;
            }

            FixCameraSize();
        }

        #if UNITY_EDITOR
        private void Update()
        {
            if (!Application.isPlaying && !updateWhileEditing)
            {
                return;
            }

            if (Application.isPlaying && !updateWhilePlaying)
            {
                return;
            }
            
            FixCameraSize();
        }
        #endif 
        
        private void FixCameraSize()
        {
            if (virtualCamera != null)
            {
                var ratioToMatchWidth = Screen.height / (float)Screen.width;
                // var ratioToMatchHeight = Screen.width / (float)Screen.height;

                var sizeToMatchWidth = widthUnits * ratioToMatchWidth * 0.5f;
                // var sizeToMatchHeight = heightUnits * ratioToMatchHeight * 0.5f;
                var sizeToMatchHeight = heightUnits / 2.0f;
                
                var widthMatch = 1.0f - match;
                var heightMatch = match;

                result = (sizeToMatchWidth * widthMatch + sizeToMatchHeight * heightMatch);
                virtualCamera.m_Lens.OrthographicSize = result;
            }
        }
    }
}