using System.Collections.Generic;
using System.Linq;
using Gemserk.RefactorTools.Editor;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Utilities.Editor
{
    [CustomPropertyDrawer(typeof(ObjectTypeAttribute), true)]
    public class ObjectTypeAttributePropertyDrawer : PropertyDrawer
    {
        private const float elementHeight = 18f;
        private List<Object> options = new List<Object>();
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.ObjectReference)
            {
                EditorGUI.LabelField(position, $"Invalid usage of ObjectTypeAttribute for field \"{label.text}\", use with Object references.");
                return;
            }

            var objectTypeAttribute = attribute as ObjectTypeAttribute;
            var typeToSelect = objectTypeAttribute.typeToSelect;
            
            EditorGUI.BeginChangeCheck();

            var buttonPosition = new Rect(position.x, position.y, position.width, elementHeight);
            var optionsPosition = new Rect(position.x, position.y + elementHeight, position.width, elementHeight);
            var objectPosition = new Rect(position.x, position.y + elementHeight * 2, position.width, elementHeight);
            
            if (GUI.Button(buttonPosition, "Load"))
            {
                options.Clear();
                
                var prefabsWithType = AssetDatabaseExt.FindPrefabs(new []{typeToSelect}, AssetDatabaseExt.FindOptions.ConsiderInactiveChildren, new []
                {
                    "Assets"
                });
                
                options.AddRange(prefabsWithType);

                var assets = AssetDatabaseExt.FindAssetsAll(typeToSelect, null, new[] { "Assets" });
                options.AddRange(assets);
            }

            if (options.Count > 0)
            {
                var selectedIndex = options.IndexOf(property.objectReferenceValue);
                var newIndex = EditorGUI.Popup(optionsPosition, selectedIndex, options.Select(o => $"{AssetDatabase.GetAssetPath(o)}").ToArray());
                
                if (newIndex >= 0 && newIndex < options.Count)
                {
                    property.objectReferenceValue = options[newIndex];
                }
            }
            else
            {
                EditorGUI.LabelField(optionsPosition, "Not loaded");
            }

            EditorGUI.ObjectField(objectPosition, property);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * 3; 
        }
    }
}