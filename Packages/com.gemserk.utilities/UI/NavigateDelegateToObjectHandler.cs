using UnityEngine;
using UnityEngine.EventSystems;

namespace Gemserk.Utilities.UI
{
    public class NavigateDelegateToObjectHandler : MonoBehaviour, IMoveHandler
    {
        public GameObject delegateObject;
        
        public void OnMove(AxisEventData eventData)
        {
            ExecuteEvents.ExecuteHierarchy(delegateObject, eventData, ExecuteEvents.moveHandler);
        }
    }
}