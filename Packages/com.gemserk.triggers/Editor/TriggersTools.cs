using UnityEngine;

namespace Gemserk.Triggers.Editor
{
    public static class TriggersTools
    {
        [UnityEditor.MenuItem("Tools/Triggers/Create Triggers Root")]
        public static void CreateTriggersRoot()
        {
            var triggerSystems = GameObject.FindObjectsByType<TriggerSystem>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            
            // Don't create two systems.
            if (triggerSystems.Length > 0)
            {
                return;
            }

            var triggerSystemObject = new GameObject("~TriggerSystem");
            triggerSystemObject.AddComponent<TriggerSystem>();
        }
    }
}