using System;
using Gemserk.Utilities.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI
{
    public class LastSelectedUIObjectData : MonoBehaviour
    {
        [NonSerialized]
        public GameObject previousSelectedObject;

        public void StoreSelectedObject()
        {
            if (EventSystem.current)
            {
                previousSelectedObject = EventSystem.current.currentSelectedGameObject;
            }
        }
        
        public void StoreSelectedObject(GameObject selectedGameObject)
        {
            previousSelectedObject = selectedGameObject;
        }
        
        public void TryRestorePreviouslySelectedObject()
        {
            TryRestorePreviouslySelectedObject(false);
        }
        
        public void TryRestorePreviouslySelectedObject(bool delayed)
        {
            if (previousSelectedObject)
            {
                if (delayed)
                {
                    StartCoroutine(InputEventSystemUtils.DelegateSelectionDelayed(previousSelectedObject));
                }
                else
                {
                    if (EventSystem.current)
                    {
                        EventSystem.current.SetSelectedGameObject(previousSelectedObject);
                    }
                }
            }
        }
    }
}