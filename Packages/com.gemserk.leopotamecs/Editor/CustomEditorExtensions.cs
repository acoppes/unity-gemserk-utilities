using System.Linq;
using UnityEditor;

namespace Gemserk.Leopotam.Ecs.Editor
{
    public static class CustomEditorExtensions
    {
        public static void DrawInspectorExcept(this SerializedObject serializedObject, string[] fieldsToSkip)
        {
            serializedObject.Update();
            SerializedProperty prop = serializedObject.GetIterator();
            if (prop.NextVisible(true))
            {
                do
                {
                    if (fieldsToSkip.Any(prop.name.Contains))
                        continue;
 
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(prop.name), true);
                }
                while (prop.NextVisible(false));
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}