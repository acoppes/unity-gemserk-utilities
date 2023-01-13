using UnityEditor;
using UnityEngine;

namespace Gemserk.Gameplay.Editor
{
    [CustomPropertyDrawer(typeof(Cooldown))]
    public class CooldownCustomEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            EditorGUI.PropertyField(position, property.FindPropertyRelative("total"), GUIContent.none);
            EditorGUI.EndProperty();
        }
    }
}