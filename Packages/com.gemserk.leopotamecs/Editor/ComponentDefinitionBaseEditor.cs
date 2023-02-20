using Gemserk.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Editor
{
    [CustomEditor(typeof(ComponentDefinitionBase), true)]
    public class ComponentDefinitionBaseEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // DrawDefaultInspector();

            var componentDefinition = target as ComponentDefinitionBase;
            
            var style = new GUIStyle(GUI.skin.label);
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = Color.grey;

            EditorGUILayout.LabelField($"<< {componentDefinition.GetComponentName()} >>", style);

            CustomEditorExtensions.DrawInspectorExcept(serializedObject, new []{ "m_Script" });
        }
    }
}