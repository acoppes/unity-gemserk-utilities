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
            }
        }

        private void OnDestroy()
        {
            if (window)
            {
                window.onOpenAction.RemoveListener(OnWindowOpen);
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
            
            StartCoroutine(InputEventSystemUtils.DelegateSelectionDelayed(objectToAutoSelect));
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