using UnityEngine;
using UnityEngine.EventSystems;

namespace Gemserk.Utilities.UI
{
    public class SelectObjectOnSelectedHandler : MonoBehaviour, ISelectHandler
    {
        public GameObject delegateObject;
        
        public void OnSelect(BaseEventData eventData)
        {
            if (delegateObject == gameObject)
                return;
            
            if (!EventSystem.current) 
                return;
            
            StartCoroutine(InputEventSystemUtils.DelegateSelectionDelayed(gameObject, delegateObject));
        }
    }
}