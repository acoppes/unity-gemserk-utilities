using Gemserk.Utilities.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI
{
    public class SelectObjectOnWindowOpen : MonoBehaviour
    {
        public UIWindow window;
        public GameObject objectToAutoSelect;
        
        private GameObject previousSelectedObject;

        private void Awake()
        {
            if (window)
            {
                window.onOpenAction.AddListener(OnWindowOpen);
                window.onCloseAction.AddListener(OnWindowClosed);
            }
        }

        private void OnDestroy()
        {
            if (window)
            {
                window.onOpenAction.RemoveListener(OnWindowOpen);
                window.onCloseAction.RemoveListener(OnWindowClosed);
            }
        }

        private void OnWindowOpen()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }
            
            if (EventSystem.current)
            {
                previousSelectedObject = EventSystem.current.currentSelectedGameObject;
            }

            var lastSelected = GetComponent<LastSelectedUIObjectData>();
            if (lastSelected && lastSelected.previousSelectedObject)
            {
                lastSelected.TryRestorePreviouslySelectedObject();
                return;
            }
            
            StartCoroutine(InputEventSystemUtils.DelegateSelectionDelayed(objectToAutoSelect));
        }
        
        private void OnWindowClosed()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            var lastSelected = GetComponent<LastSelectedUIObjectData>();
            if (lastSelected)
            {
                lastSelected.StoreSelectedObject();
            }
        }
        
        public void TryRestorePreviouslySelectedObject()
        {
            if (previousSelectedObject)
            {
                if (EventSystem.current)
                {
                    EventSystem.current.SetSelectedGameObject(previousSelectedObject);
                }
            }
        }
    }
}