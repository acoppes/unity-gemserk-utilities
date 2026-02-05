using Gemserk.Utilities;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Triggers.Editor
{
    [UnityEditor.InitializeOnLoad]
    public static class TriggerHierarchyNaming
    {
        static TriggerHierarchyNaming()
        {
            EditorApplication.update += Update;
        }

        private static void Update()
        {
            if (EditorApplication.isPlaying)
                return;
            var triggerSystems= GameObject.FindObjectsByType<TriggerSystem>(FindObjectsInactive.Exclude,
                FindObjectsSortMode.None);

            foreach (var triggerSystem in triggerSystems)
            {
                var debugNamedObjects = triggerSystem.GetComponentsInChildren<IDebugNamedObject>();
                foreach (var debugNamedObject in debugNamedObjects)
                {
                    if (debugNamedObject is MonoBehaviour m)
                    {
                        var objectName = debugNamedObject.GetObjectName();
                        if (!string.IsNullOrEmpty(objectName))
                        {
                            m.gameObject.name = debugNamedObject.GetObjectName();
                        }
                    }
                }
            }
        }
    }
}