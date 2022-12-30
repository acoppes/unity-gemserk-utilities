using UnityEditor;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Editor
{
    [CustomPropertyDrawer(typeof(TypeValidationAttribute), true)]
    public class TypeValidationPropertyDrawer: PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
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