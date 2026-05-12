using System;
using UnityEngine;
using UnityEngine.Events;

namespace Gemserk.Utilities.UI
{
    public class UIWindowAnimationEvents : MonoBehaviour
    {
        public UnityEvent onOpenAnimationStarted;
        public UnityEvent onOpenAnimationCompleted;
        
        public UnityEvent onClosedAnimationStarted;
        public UnityEvent onClosedAnimationCompleted;

        [NonSerialized]
        public bool animating;
        
        public void OnWindowOpenStarted()
        {
            onOpenAnimationStarted.Invoke();
            animating = true;
        }
        
        public void OnWindowOpened()
        {
            onOpenAnimationCompleted.Invoke();
            animating = false;
        }
        
        public void OnWindowCloseStarted()
        {
            onClosedAnimationStarted.Invoke();
            animating = true;
        }

        public void OnWindowClosed()
        {
            onClosedAnimationCompleted.Invoke();
            animating = false;
        }
    }
}