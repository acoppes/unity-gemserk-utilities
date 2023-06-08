using UnityEditor;
using UnityEngine;

namespace Gemserk.Utilities.Editor
{
    [CustomPropertyDrawer(typeof(SelectablePath), true)]
    public class SelectablePathDrawer: PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var pathProperty = property.FindPropertyRelative("path");
            var path = pathProperty.stringValue;

            var content = EditorGUIUtility.IconContent("Folder Icon");
            
            var buttonPosition = position;
            buttonPosition.width = 30;

            var defaultPosition = position;
            defaultPosition.width = position.width - 80;
            
            buttonPosition.x = position.width - 15;

            EditorGUI.BeginProperty(position, label, pathProperty);
            EditorGUI.PropertyField(defaultPosition, pathProperty);

            if (GUI.Button(buttonPosition, content))
            {
                var newPath = GUIInternalUtils.SelectLocalFolder(path);
     
                if (!string.IsNullOrEmpty(newPath))
                {
                    pathProperty.stringValue = newPath;
                    property.serializedObject.ApplyModifiedProperties();
                }
                
                GUIUtility.ExitGUI();
            }
            
            EditorGUI.EndProperty();
        }
    }
}