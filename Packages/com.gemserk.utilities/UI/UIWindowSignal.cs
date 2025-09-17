using Gemserk.Utilities.Signals;
using UnityEngine;

namespace Gemserk.Utilities.UI
{
    public class UIWindowSignal : MonoBehaviour
    {
        public UIWindow window;

        public SignalAsset windowOpenSignal;
        public SignalAsset windowClosedSignal;
        
        private void OnEnable()
        {
            window.onOpenAction.AddListener(OnWindowOpen);
            window.onCloseAction.AddListener(OnWindowClosed);
        }


        private void OnDisable()
        {
            window.onOpenAction.RemoveListener(OnWindowOpen);
            window.onCloseAction.RemoveListener(OnWindowClosed);
        }
        
        private void OnWindowOpen()
        {
            windowOpenSignal.Signal(gameObject);
        }
        
        private void OnWindowClosed()
        {
            windowClosedSignal.Signal(gameObject);
        }

    }
}