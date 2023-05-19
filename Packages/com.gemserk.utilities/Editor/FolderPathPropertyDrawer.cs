using System.IO;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Utilities.Editor
{
    [CustomPropertyDrawer(typeof(FolderPathAttribute), true)]
    public class FolderPathPropertyDrawer: PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var path = property.stringValue;

            var content = EditorGUIUtility.IconContent("Folder Icon");
            
            var buttonPosition = position;
            buttonPosition.width = 30;

            var defaultPosition = position;
            defaultPosition.width = position.width - 40;
            
            buttonPosition.x = position.width - 15;

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PropertyField(defaultPosition, property);

            if (GUI.Button(buttonPosition, content))
            {
                var previousPath = Path.Combine(Application.dataPath, path);
                
                var newPath = EditorUtility.OpenFolderPanel("Select folder", previousPath, "");
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