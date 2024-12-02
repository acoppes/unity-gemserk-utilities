using UnityEngine;

namespace Game.Utilities
{
    public class CursorModeCapture : MonoBehaviour
    {
        public bool visible;
        public CursorLockMode lockMode = CursorLockMode.None;
        
        public void Start()
        {
            Cursor.visible = visible;
            Cursor.lockState = lockMode;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                Cursor.visible = visible;
                Cursor.lockState = lockMode;
            }
        }
    }
}