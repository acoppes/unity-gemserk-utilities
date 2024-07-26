using System;
using System.Collections.Generic;
using System.Linq;
using Gemserk.RefactorTools.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gemserk.Utilities.Editor
{
    [CustomPropertyDrawer(typeof(BaseObjectTypeAttribute), true)]
    public class ObjectTypeAttributePropertyDrawer : PropertyDrawer
    {
        private const float elementHeight = 20f;
        private List<SelectReferenceWindow.ObjectReference> options = new List<SelectReferenceWindow.ObjectReference>();

        private Object lastSelectedObject;

        protected virtual SerializedPropertyType GetValidPropertyType()
        {
            return SerializedPropertyType.ObjectReference;
        }

        protected virtual Type GetTypeToSelect(SerializedProperty property)
        {
            var objectTypeAttribute = attribute as BaseObjectTypeAttribute;
            return objectTypeAttribute.GetPropertyType();
        }
        
        protected virtual SerializedProperty GetPropertyToOverride(SerializedProperty property)
        {
            return property;
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != GetValidPropertyType())
            {
                EditorGUI.LabelField(position, $"Invalid usage of ObjectTypeAttribute for field \"{label.text}\", use with Object references.");
                return;
            }

            var objectTypeAttribute = attribute as BaseObjectTypeAttribute;
            var typeToSelect = GetTypeToSelect(property);
            var objectProperty = GetPropertyToOverride(property);
            
            EditorGUI.BeginChangeCheck();

            var labelPosition = new Rect(position.x, position.y, position.width * 0.25f, elementHeight * 1);
          //  var optionsPosition = new Rect(position.x, position.y + elementHeight, position.width, elementHeight);
            var objectPosition = new Rect(position.x + position.width * 0.25f, position.y + elementHeight * 0, position.width * 0.6f, elementHeight);
            var buttonPosition = new Rect(position.x + position.width * 0.85f, position.y, position.width * 0.15f, elementHeight);

            if (lastSelectedObject != null)
            {
                objectProperty.objectReferenceValue = lastSelectedObject;
                lastSelectedObject = null;
            }
            
            EditorGUI.LabelField(labelPosition, objectProperty.displayName);
            
            if (GUI.Button(buttonPosition, "Select"))
            {
                options.Clear();

                if (!objectTypeAttribute.disableSceneReferences)
                {
                    var sceneObjects = Object.FindObjectsByType(typeof(Component), FindObjectsInactive.Exclude,
                        FindObjectsSortMode.None);
                    var filteredSceneObjects = sceneObjects.Where(c => typeToSelect.IsInstanceOfType(c));
                    options.AddRange(filteredSceneObjects.Select(o => new SelectReferenceWindow.ObjectReference()
                    {
                        reference = o,
                        source = SelectReferenceWindow.ObjectReference.Source.Scene
                    }));
                }

                if (!objectTypeAttribute.disablePrefabReferences)
                {
                    var prefabsWithType = AssetDatabaseExt.FindPrefabs(new[] { typeToSelect }, 
                        AssetDatabaseExt.FindOptions.ConsiderChildren, objectTypeAttribute.filterString, new[]
                        {
                            "Assets"
                        });

                    options.AddRange(prefabsWithType.Select(o => new SelectReferenceWindow.ObjectReference()
                    {
                        reference = o,
                        source = SelectReferenceWindow.ObjectReference.Source.Prefab
                    }));
                }

                if (!objectTypeAttribute.disableAssetReferences)
                {
                    var assets = AssetDatabaseExt.FindAssetsAll(typeToSelect, objectTypeAttribute.filterString, new[] { "Assets" });
                    // options.AddRange(assets);
                    
                    options.AddRange(assets.Select(o => new SelectReferenceWindow.ObjectReference()
                    {
                        reference = o,
                        source = SelectReferenceWindow.ObjectReference.Source.Asset
                    }));
                }
                
                SelectReferenceWindow.OpenWindow(options, new SelectReferenceWindow.Configuration()
                {
                    canSelectAssetReferences = !objectTypeAttribute.disableAssetReferences,
                    canSelectPrefabReferences = !objectTypeAttribute.disablePrefabReferences,
                    canSelectSceneReferences = !objectTypeAttribute.disableSceneReferences,
                    onSelectReferenceCallback = OnReferenceObjectSelected
                });
            }

            var previousObject = objectProperty.objectReferenceValue;
            EditorGUI.BeginChangeCheck();
            var newObject = EditorGUI.ObjectField(objectPosition, GUIContent.none, previousObject, typeof(Object), true);
            if (EditorGUI.EndChangeCheck())
            {
                var validType = typeToSelect.IsInstanceOfType(newObject);
                
                if (!validType && newObject is GameObject go)
                {
                    validType = go.GetComponentInChildren(typeToSelect, false) != null;
                }
                
                if (validType || newObject == null)
                {
                    objectProperty.objectReferenceValue = newObject;
                }
            }
        }

        private void OnReferenceObjectSelected(SelectReferenceWindow.ObjectReference objectReference)
        {
            lastSelectedObject = objectReference.reference;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * 1; 
        }
    }
    
    [CustomPropertyDrawer(typeof(InterfaceReferenceTypeAttribute), true)]
    public class InterfaceReferenceTypeAttributePropertyDrawer : ObjectTypeAttributePropertyDrawer
    {
        protected override SerializedPropertyType GetValidPropertyType()
        {
            return SerializedPropertyType.Generic;
        }
        
        protected override Type GetTypeToSelect(SerializedProperty property)
        {
            var boxedValue = property.boxedValue;
            var objectType = boxedValue.GetType();
            var genericTypes = objectType.GetGenericArguments();
            return genericTypes[0];
        }
        
        protected override SerializedProperty GetPropertyToOverride(SerializedProperty property)
        {
            return property.FindPropertyRelative("source");
        }
    }
}