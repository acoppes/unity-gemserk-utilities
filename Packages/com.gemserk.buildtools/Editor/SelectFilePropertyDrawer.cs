using UnityEditor;
using UnityEngine;

namespace Gemserk.BuildTools.Editor
{
    [CustomPropertyDrawer(typeof(SelectFileAttribute), true)]
    public class SelectFilePropertyDrawer: PropertyDrawer
    {
        // private float height = 20;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var path = property.stringValue;

            var selectFileAttribute = attribute as SelectFileAttribute;

            var content = EditorGUIUtility.IconContent("d_Folder Icon");
            
            var buttonPosition = position;
            buttonPosition.width = 30;

            var defaultPosition = position;
            defaultPosition.width = position.width - 40;
            
            buttonPosition.x = position.width - 15;

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PropertyField(defaultPosition, property);

            // content.image.width;
            
            if (GUI.Button(buttonPosition, content))
            {
                if (selectFileAttribute.isFolder)
                {
                    var newPath = EditorUtility.OpenFolderPanel("Select folder", path, "");
                    if (!string.IsNullOrEmpty(newPath))
                    {
                        property.stringValue = newPath;
                        property.serializedObject.ApplyModifiedProperties();
                    }
                    GUIUtility.ExitGUI();
                }
                else
                {
                    var newPath = EditorUtility.OpenFilePanel("Select folder", path, "");
                    if (!string.IsNullOrEmpty(newPath))
                    {
                        property.stringValue = newPath; 
                        property.serializedObject.ApplyModifiedProperties();
                    }
                    GUIUtility.ExitGUI();
                }
            }
            
            EditorGUI.EndProperty();
            
        }

        // public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        // {
        //     return height * 2;
        // }
    }
}