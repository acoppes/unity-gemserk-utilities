using UnityEngine;
using UnityEngine.UI;

namespace Gemserk.Utilities.UI
{
    public class CanvasToolsDelegate : MonoBehaviour
    {
        public void ForceRefresh()
        {
#if UNITY_6000_4_OR_NEWER
            var graphicsArray = FindObjectsByType<Graphic>(FindObjectsInactive.Include);
#else
            var graphicsArray = FindObjectsByType<Graphic>(FindObjectsInactive.Include, FindObjectsSortMode.None);
#endif
            
            foreach (var graphic in graphicsArray)
            {
                graphic.SetAllDirty();
            }
        }

    }
}