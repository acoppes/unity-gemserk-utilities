using UnityEngine;
using Vertx.Debugging;

namespace Game.LevelDesign
{
    public class DebugLabelObject : MonoBehaviour
    {
        public Color color1, color2;
        
        public string customText;
        
        public bool disableShowName;
     
        private void OnDrawGizmos()
        {
            if (!isActiveAndEnabled)
                return;
            
            #if UNITY_EDITOR
            if (!disableShowName)
            {
                var text = customText;

                if (string.IsNullOrEmpty(customText))
                {
                    text = gameObject.name;
                }
                
                D.raw(new Shape.Text(transform.position, text), color1, color2);
            }
            #endif
        }
    }
}