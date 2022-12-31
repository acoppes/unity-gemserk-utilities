using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestCustomInspectorClean), true)]
public class TestCustomInspectorCleanEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        // var property = serializedObject.GetIterator();
        //
        // var enterChildren = true;
        //
        // while (property.Next(enterChildren))
        // {
        //     if (property.pr)
        //     EditorGUILayout.PropertyField(property);
        //     enterChildren = false;
        // }

        if (GUILayout.Button("Remove"))
        {
            
        }
    }
}