using Gemserk.Utilities.UI;
using UnityEngine;

namespace Game.UI
{
    public class OnWindowOpenPauseGame : MonoBehaviour
    {
        public UIWindow window;
        
        private void OnEnable()
        {
            if (window)
            {
                window.onOpenAction.AddListener(OnWindowOpen);
                window.onCloseAction.AddListener(OnWindowClosed);
            }
        }

        private void OnDisable()
        {
            if (window)
            {
                window.onOpenAction.RemoveListener(OnWindowOpen);
                window.onCloseAction.RemoveListener(OnWindowClosed);
            }
        }

        private static void OnWindowOpen()
        {
            GamePause.Pause();
        }
        
        private static void OnWindowClosed()
        {
            GamePause.Resume();
        }
    }
}