using UnityEditor;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Editor
{
    // [CustomPropertyDrawer(typeof(IEntityComponentDefinition), true)]
    // public class EntityComponentDefinitionDrawer: PropertyDrawer
    // {
    //     public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //     {
    //         // if (property.propertyType != SerializedPropertyType.ManagedReference)
    //         // {
    //         //     return;
    //         // }
    //         
    //         foreach (SerializedProperty a in property)
    //         {            
    //             if (a.depth < 3)
    //             {
    //                 position.y += EditorGUI.GetPropertyHeight(a);
    //                 EditorGUI.PropertyField(position, a, new GUIContent(a.name));
    //             }
    //         }
    //
    //         // EditorGUI.PropertyField(position, property, label);
    //     }
    //
    //     public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    //     {
    //         var height = EditorGUI.GetPropertyHeight(property);
    //
    //         foreach (SerializedProperty a in property)
    //         {
    //             if (a.depth < 3)
    //             {
    //                 height += EditorGUI.GetPropertyHeight(a);
    //             }
    //             // position.y += 20;
    //             // EditorGUI.PropertyField(position, a, new GUIContent(a.name));
    //         }
    //         
    //         return height;
    //     }
    // }
}