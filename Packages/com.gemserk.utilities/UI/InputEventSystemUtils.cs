using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gemserk.Utilities.UI
{
    public static class InputEventSystemUtils
    {
        public static IEnumerator DelegateSelectionDelayed(GameObject current, GameObject newSelection)
        {
            yield return null;
            
            // check if this window is still the selected one
            if (EventSystem.current && newSelection && EventSystem.current.currentSelectedGameObject == current)
            {
                EventSystem.current.SetSelectedGameObject(newSelection);
            }
        }
    }
}