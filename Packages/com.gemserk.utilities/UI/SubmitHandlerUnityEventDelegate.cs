using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Gemserk.Utilities.UI
{
    public class SubmitHandlerUnityEventDelegate : MonoBehaviour, ISubmitHandler
    {
        public UnityEvent onSubmit;

        public void OnSubmit(BaseEventData eventData)
        {
            onSubmit.Invoke();
        }
    }
}