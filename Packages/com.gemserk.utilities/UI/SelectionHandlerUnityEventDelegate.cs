using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Gemserk.Utilities.UI
{
    public class SelectionHandlerUnityEventDelegate : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        public UnityEvent onSelected, onDeselected;
        
        public void OnSelect(BaseEventData eventData)
        {
            onSelected.Invoke();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            onDeselected.Invoke();
        }
    }
}