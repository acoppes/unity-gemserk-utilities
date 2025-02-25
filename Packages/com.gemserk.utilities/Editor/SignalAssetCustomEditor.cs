using Gemserk.Utilities.Signals;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Utilities.Editor
{
    [CustomEditor(typeof(SignalAsset), true)]
    public class SignalAssetCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var signalAsset = target as SignalAsset;

            var handlers = signalAsset.GetHandlers();
            
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("TOTAL", $"{handlers.Count}");
            EditorGUI.BeginDisabledGroup(!Application.isPlaying || handlers.Count == 0);
            if (GUILayout.Button("Signal"))
            {
                signalAsset.Signal(null);
            }
            EditorGUI.EndDisabledGroup();
            
            EditorGUI.indentLevel++;
            foreach (var handler in handlers)
            {
                var target = handler.Target;
                if (target is Object unityObject)
                {
                    // EditorGUILayout.LabelField(unityObject.name, handler.Method.Name);
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.ObjectField(unityObject, typeof(Object));
                    EditorGUI.EndDisabledGroup();
                }
                else
                {
                    EditorGUILayout.LabelField(handler.Method.Name);
                }
                
            }
            EditorGUI.indentLevel--;

            if (handlers.Count > 0)
            {
                if (GUILayout.Button("Clear"))
                {
                    signalAsset.Clear();
                }
            }
            
            EditorGUILayout.EndVertical();
        }
    }
}