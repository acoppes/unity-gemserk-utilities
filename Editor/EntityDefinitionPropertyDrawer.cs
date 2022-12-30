using UnityEditor;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Editor
{
    [CustomPropertyDrawer(typeof(EntityDefinitionAttribute))]
    public class EntityDefinitionPropertyDrawer: PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.ObjectField(position, property, label);
            if (EditorGUI.EndChangeCheck())
            {
                var referencedObject =  property.objectReferenceValue;
                
                if (referencedObject is IEntityDefinition)
                {
                    return;
                }
                
                if (referencedObject is GameObject gameObject)
                {
                    var isEntityDefinition = gameObject.GetComponentInChildren<IEntityDefinition>() != null;
                    if (!isEntityDefinition)
                    {
                        property.objectReferenceValue = null;
                        Debug.Log("Invalid object, not an EntityDefinition.");
                    }
                }
                else
                {
                    property.objectReferenceValue = null;
                    Debug.Log("Invalid object, not an EntityDefinition.");
                }
            }
            
            
        }
    }
}