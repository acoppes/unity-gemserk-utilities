using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Gemserk.Triggers.Editor
{
    [InitializeOnLoad]
    public static class TriggerSystemHierarchyNaming
    {
        public static bool AutoNamingDisabled = false;
        
        private const double UpdateFrequency = 1;
        private static double _lastUpdateTime;
        
        static TriggerSystemHierarchyNaming()
        {
            EditorApplication.update += Update;
        }

        private static void Update()
        {
            if (EditorApplication.isPlaying)
                return;

            if (AutoNamingDisabled)
                return;

            var timeSinceStartup = EditorApplication.timeSinceStartup;

            if (timeSinceStartup - _lastUpdateTime < UpdateFrequency)
            {
                return;
            }

            _lastUpdateTime = timeSinceStartup;

            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();

            var debugNamedObjects = new List<ITriggerDebugNamedObject>();
            
            if (prefabStage)
            {
                prefabStage.prefabContentsRoot.GetComponentsInChildren(debugNamedObjects);
            }
            else
            {
                var triggerSystems= GameObject.FindObjectsByType<TriggerSystem>(FindObjectsInactive.Exclude,
                    FindObjectsSortMode.None);
                foreach (var triggerSystem in triggerSystems)
                {
                    debugNamedObjects.AddRange(triggerSystem.GetComponentsInChildren<ITriggerDebugNamedObject>());
                }
            }

            foreach (var debugNamedObject in debugNamedObjects)
            {
                if (debugNamedObject is MonoBehaviour m)
                {
                    var objectName = debugNamedObject.GetObjectName();
                    if (!string.IsNullOrEmpty(objectName))
                    {
                        if (!string.Equals(m.gameObject.name, objectName, StringComparison.InvariantCulture))
                        {
                            m.gameObject.name = objectName;
                            EditorUtility.SetDirty(m.gameObject);
                        }
                    }
                }
            }
        }
    }
}