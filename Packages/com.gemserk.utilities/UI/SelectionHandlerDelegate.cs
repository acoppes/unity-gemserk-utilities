using UnityEngine;
using UnityEngine.EventSystems;

namespace Gemserk.Utilities.UI
{
    public class SelectionHandlerDelegate : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        public GameObject delegateObject;
        
        public void OnSelect(BaseEventData eventData)
        {
            ExecuteEvents.Execute(delegateObject, eventData, ExecuteEvents.selectHandler);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            ExecuteEvents.Execute(delegateObject, eventData, ExecuteEvents.deselectHandler);
        }
    }
}