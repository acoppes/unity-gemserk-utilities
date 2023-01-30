using UnityEditor;
using UnityEngine;

namespace Gemserk.Triggers.Editor
{
    [CustomEditor(typeof(TriggerSystem), true)]
    public class TriggerSystemCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var triggerSystem = target as TriggerSystem;
            
            if (!Application.isPlaying)
            {
                if (GUILayout.Button("Create Trigger"))
                {
                    var triggerObject = new GameObject("Trigger");
                    triggerObject.transform.SetParent(triggerSystem.transform);
                    
                    var trigger = triggerObject.AddComponent<TriggerObject>();
                    Selection.activeObject = triggerObject;
                    
                    // trigger.ValidateParents();
                    
                    EditorUtility.SetDirty(triggerObject);
                }
            }
        }
    }
}