using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Gemserk.Utilities.UI
{
    public class CancelHandlerUnityEventDelegate : MonoBehaviour, ICancelHandler
    {
        public UnityEvent onCancel;
        
        public void OnCancel(BaseEventData eventData)
        {
            onCancel.Invoke();
        }
    }
}