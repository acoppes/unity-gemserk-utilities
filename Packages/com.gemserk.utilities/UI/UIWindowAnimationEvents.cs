using UnityEngine;
using UnityEngine.Events;

namespace Gemserk.Utilities.UI
{
    public class UIWindowAnimationEvents : MonoBehaviour
    {
        public UnityEvent onOpenAnimationCompleted;
        public UnityEvent onClosedAnimationCompleted;
        
        public void OnWindowOpened()
        {
            onOpenAnimationCompleted.Invoke();
        }

        public void OnWindowClosed()
        {
            onClosedAnimationCompleted.Invoke();
        }
    }
}