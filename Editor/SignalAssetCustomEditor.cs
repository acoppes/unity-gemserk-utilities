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
            EditorGUI.BeginChangeCheck();
            DrawDefaultInspector();
            
            var defaultInspectorChanged = EditorGUI.EndChangeCheck();

            var signalAsset = target as SignalAsset;

            var handlers = signalAsset.GetHandlers();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("TOTAL", $"{handlers.Count}");
            EditorGUI.indentLevel++;
            foreach (var handler in handlers)
            {
                var target = handler.Target;
                if (target is Object unityObject)
                {
                    EditorGUILayout.LabelField(unityObject.name, handler.Method.Name);
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
            EditorGUILayout.EndVertical();
        }
    }
}