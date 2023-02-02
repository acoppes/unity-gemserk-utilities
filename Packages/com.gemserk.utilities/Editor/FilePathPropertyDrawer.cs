using System.IO;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Utilities.Editor
{
    [CustomPropertyDrawer(typeof(FilePathAttribute), true)]
    public class FilePathPropertyDrawer: PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var path = property.stringValue;

            var content = EditorGUIUtility.IconContent("d_Folder Icon");
            
            var buttonPosition = position;
            buttonPosition.width = 30;

            var defaultPosition = position;
            defaultPosition.width = position.width - 40;
            
            buttonPosition.x = position.width - 15;

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PropertyField(defaultPosition, property);

            if (GUI.Button(buttonPosition, content))
            {
                var newPath = EditorUtility.OpenFilePanel("Select folder", path, "");
                if (!string.IsNullOrEmpty(newPath))
                {
                    var relativePath = Path.GetRelativePath(Application.dataPath, newPath);
                        
                    property.stringValue = relativePath; 
                    property.serializedObject.ApplyModifiedProperties();
                }
                GUIUtility.ExitGUI();
            }
            
            EditorGUI.EndProperty();
            
        }
    }
}