using UnityEngine;
using UnityEngine.UI;

namespace Gemserk.Utilities.UI
{
    public class CanvasToolsDelegate : MonoBehaviour
    {
        public void ForceRefresh()
        {
            var graphicsArray = FindObjectsByType<Graphic>(FindObjectsInactive.Include);
            
            foreach (var graphic in graphicsArray)
            {
                graphic.SetAllDirty();
            }
        }

    }
}