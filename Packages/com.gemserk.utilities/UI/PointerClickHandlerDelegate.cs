using UnityEngine;
using UnityEngine.EventSystems;

namespace Gemserk.Utilities.UI
{
    public class PointerClickHandlerDelegate : MonoBehaviour, IPointerClickHandler
    {
        public enum DelegateType
        {
            Parent = 0,
            Object = 1
        }

        public DelegateType delegateType;
        public GameObject delegateObject;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (delegateType == DelegateType.Parent)
            {
                ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, eventData, ExecuteEvents.pointerClickHandler);
            }
            else if (delegateType == DelegateType.Object)
            {
                ExecuteEvents.ExecuteHierarchy(delegateObject, eventData, ExecuteEvents.pointerClickHandler);
            }
        }
    }
}