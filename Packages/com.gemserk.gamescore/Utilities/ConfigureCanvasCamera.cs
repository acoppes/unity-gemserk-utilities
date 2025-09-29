using MyBox;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Utilities
{
    public class ConfigureCanvasCamera : MonoBehaviour
    {
        public enum FindType
        {
            Tag = 0,
            // Name = 1
        }
        
        public Canvas canvas;

        public FindType findType = FindType.Tag;
        
        [ConditionalField(nameof(findType), false, FindType.Tag)]
        [Tag]
        public string cameraTag;
        
        public void Start()
        {
            var cameraObject = GameObject.FindWithTag(cameraTag);
            if (cameraObject)
            {
                canvas.worldCamera = cameraObject.GetComponent<Camera>();
                var raycaster = GetComponent<BaseRaycaster>();
                if (raycaster)
                {
                    raycaster.enabled = false;
                    raycaster.enabled = true;
                }
            }
        }
    }
}