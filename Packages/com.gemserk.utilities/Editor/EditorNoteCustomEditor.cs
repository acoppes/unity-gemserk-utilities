using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Utilities.Editor
{
    [CustomPropertyDrawer(typeof(EditorNote), true)]
    public class EditorNotePropertyDrawer : PropertyDrawer
    {
        private bool canEdit = false;
        private const int ButtonSize = 25;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var note = property.FindPropertyRelative("note");

            
            var textPosition = new Rect(position.x, position.y, position.width - ButtonSize - 5, position.height);
            var buttonPosition = new Rect(position.x + position.width - ButtonSize, position.y, ButtonSize, ButtonSize);

            var openFolderIcon = EditorGUIUtility.IconContent("TextAsset Icon");
            var selectSourceFolder = new GUIContent(openFolderIcon.image, "Edit");
            
            // GUILayout.Width(25), GUILayout.Height(EditorGUIUtility.singleLineHeight)
            
            if (GUI.Button(buttonPosition, selectSourceFolder))
            {
                canEdit = !canEdit;
            }
            
            EditorGUI.BeginDisabledGroup(!canEdit);
            note.stringValue = EditorGUI.TextArea(textPosition, note.stringValue);
            EditorGUI.EndDisabledGroup();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var note = property.FindPropertyRelative("note");
            var height = EditorGUIUtility.singleLineHeight * note.stringValue.Split('\n').Length;
            return Mathf.Max(height, ButtonSize);
        }
    }
}