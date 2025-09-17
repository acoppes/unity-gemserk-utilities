using UnityEngine;
using UnityEngine.Events;

namespace Gemserk.Utilities.UI
{
    public class UIWindow : MonoBehaviour
    {
        private static readonly int WindowOpen = Animator.StringToHash("open");
        
        // public interface IWindowOpenHandler
        // {
        //     void OnWindowOpen(UIWindow window);
        // }

        private enum State
        {
            None,
            Open,
            Closed
        }
        
        public delegate void WindowStateChangedHandler();
        
        public bool startsOpen = false;

        public GameObject canvasObject;
        public CanvasGroup canvasGroup;

        public Animator windowAnimator;

        public bool IsOpen() => state == State.Open;
        
        public bool IsClosed() => state == State.Closed;
        
        public UnityEvent onOpenAction;
        public UnityEvent onCloseAction;

        private State state = State.None;
        
        private void Start()
        {
            if (state == State.None)
            {
                if (startsOpen)
                {
                    Open(true);
                }
                else
                {
                    Close(false);
                }
            }
        }

        public void Open()
        {
            Open(true);
        }
        
        public void Open(bool callbacks)
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (IsOpen())
            {
                return;
            }

            state = State.Open;

            if (windowAnimator)
            {
                windowAnimator.SetBool(WindowOpen, true);
            } else
            {
                canvasObject.SetActive(true);
                canvasGroup.interactable = true;
                canvasGroup.alpha = 1;
                canvasGroup.blocksRaycasts = true;
            }
            
            if (callbacks)
            {
                onOpenAction.Invoke();
                // gameObject.BroadcastMessage(nameof(IWindowOpenHandler.OnWindowOpen), this, SendMessageOptions.DontRequireReceiver);
            }
        }
        
        public void Close()
        {
            Close(true);
        }

        public void Close(bool callbacks)
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (IsClosed())
            {
                return;
            }

            state = State.Closed;

            if (callbacks)
            {
                onCloseAction.Invoke();
                // gameObject.BroadcastMessage(nameof(IWindowOpenHandler.OnWindowClosed), this, SendMessageOptions.DontRequireReceiver);
            }
            
            if (windowAnimator)
            {
                windowAnimator.SetBool(WindowOpen, false);
            }
            else
            {
                canvasGroup.interactable = false;
                canvasGroup.alpha = 0;
                canvasGroup.blocksRaycasts = false;
                canvasObject.SetActive(false);
            }
        }
    }
}