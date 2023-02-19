using UnityEditor;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Editor
{
    [CustomEditor(typeof(ComponentDefinitionBase), true)]
    public class ComponentDefinitionBaseEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            // CustomEditorExtensions.DrawInspectorExcept(serializedObject, new []{ "m_Script" });
            // if (GUILayout.Button($"Remove"))
            // {
            //     GameObject.DestroyImmediate(target);
            // }
        }
    }
}