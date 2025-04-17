using UnityEngine;
using UnityEngine.EventSystems;

namespace Gemserk.Utilities.UI
{
    public class CancelHandlerParentDelegate : MonoBehaviour, ICancelHandler
    {
        public void OnCancel(BaseEventData eventData)
        {
            ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, eventData, ExecuteEvents.cancelHandler);
        }
    }
}