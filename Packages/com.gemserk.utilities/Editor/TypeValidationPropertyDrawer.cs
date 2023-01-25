using UnityEditor;
using UnityEngine;

namespace Gemserk.Utilities.Editor
{
    [CustomPropertyDrawer(typeof(TypeValidationAttribute), true)]
    public class TypeValidationPropertyDrawer: PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.ObjectReference)
            {
                EditorGUI.LabelField(position, $"Invalid usage of TypeValidation for field \"{label.text}\", use with Object references.");
                return;
            }
            
            var typeToValidate = (attribute as TypeValidationAttribute).typeToValidate;
            
            EditorGUI.BeginChangeCheck();
            EditorGUI.ObjectField(position, property, label);
            if (EditorGUI.EndChangeCheck())
            {
                var referencedObject =  property.objectReferenceValue;
                
                if (typeToValidate.IsInstanceOfType(referencedObject))
                {
                    return;
                }
                
                if (referencedObject is GameObject gameObject)
                {
                    var isValidType = gameObject.GetComponentInChildren(typeToValidate) != null;
                    if (!isValidType)
                    {
                        property.objectReferenceValue = null;
                        Debug.Log($"Invalid object, not an {typeToValidate.Name}.");
                    }
                }
                else
                {
                    property.objectReferenceValue = null;
                    Debug.Log($"Invalid object, not an {typeToValidate.Name}.");
                }
            }
        }
    }
}