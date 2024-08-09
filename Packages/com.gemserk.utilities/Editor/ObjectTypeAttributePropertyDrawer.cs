using System;
using System.Collections.Generic;
using System.Linq;
using Gemserk.RefactorTools.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gemserk.Utilities.Editor
{
    public class ObjectTypeAttributeGUI
    {
        public class Options
        {
            public string filterString = null;
            public string[] folders;
            public FindObjectsInactive sceneReferencesFilter = FindObjectsInactive.Include;
            
            public bool sceneReferencesOpen;
            public bool prefabReferencesOpen;
            public bool assetReferencesOpen;
            
            public bool disableSceneReferences;
            public bool disablePrefabReferences;
            public bool disableAssetReferences;
        }
        
        private const float ElementHeight = 20f;
        private Object lastSelectedObject;
        
        public void DrawGUI(Rect position, SerializedProperty objectProperty, Type typeToSelect, Options options)
        {
            EditorGUI.BeginProperty(position, new GUIContent(objectProperty.displayName), objectProperty);
            
            EditorGUI.BeginChangeCheck();

            var labelPosition = new Rect(position.x, position.y, position.width * 0.25f, ElementHeight * 1);
          //  var optionsPosition = new Rect(position.x, position.y + elementHeight, position.width, elementHeight);
            var objectPosition = new Rect(position.x + position.width * 0.25f, position.y + ElementHeight * 0, position.width * 0.6f, ElementHeight);
            var buttonPosition = new Rect(position.x + position.width * 0.85f, position.y, position.width * 0.15f, ElementHeight);

            if (lastSelectedObject != null)
            {
                objectProperty.objectReferenceValue = lastSelectedObject;
                lastSelectedObject = null;
            }
            
            EditorGUI.LabelField(labelPosition, objectProperty.displayName);
            
            if (GUI.Button(buttonPosition, "Select"))
            {
                // options.Clear();

                Func<List<SelectReferenceWindow.ObjectReference>> getSceneReferences = () =>
                {
                    var sceneReferences = new List<SelectReferenceWindow.ObjectReference>();

                    var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
                    IEnumerable<Object> filteredSceneObjects = null; 

                    if (prefabStage != null)
                    {
                        filteredSceneObjects = prefabStage.prefabContentsRoot.GetComponentsInChildren(typeToSelect);
                    }
                    else
                    {
                        var sceneObjects = Object.FindObjectsByType(typeof(Component), options.sceneReferencesFilter,
                            FindObjectsSortMode.None);
                        filteredSceneObjects = sceneObjects.Where(typeToSelect.IsInstanceOfType);
                    }
                    
                    if (!string.IsNullOrEmpty(options.filterString))
                    {
                        filteredSceneObjects = filteredSceneObjects.Where(c => c.name.ToLower().Contains(options.filterString.ToLower()));
                    }
                    sceneReferences.AddRange(filteredSceneObjects.Select(o => new SelectReferenceWindow.ObjectReference()
                    {
                        reference = o,
                        source = SelectReferenceWindow.ObjectReference.Source.Scene
                    }));
                    return sceneReferences;
                };

                var folders = options.folders;
                
                Func<List<SelectReferenceWindow.ObjectReference>> getPrefabReferences = () =>
                {
                    var prefabReferences = new List<SelectReferenceWindow.ObjectReference>();
                    var prefabsWithType = AssetDatabaseExt.FindPrefabs(new[] { typeToSelect }, 
                        AssetDatabaseExt.FindOptions.ConsiderChildren, options.filterString, folders);
                    prefabReferences.AddRange(prefabsWithType.Select(o => new SelectReferenceWindow.ObjectReference()
                    {
                        reference = o,
                        source = SelectReferenceWindow.ObjectReference.Source.Prefab
                    }));
                    return prefabReferences;
                };
                
                Func<List<SelectReferenceWindow.ObjectReference>> getAssetReferences = () =>
                {
                    var assetReferences = new List<SelectReferenceWindow.ObjectReference>();
                    var assets = AssetDatabaseExt.FindAssetsAll(typeToSelect, options.filterString, folders);
                    assetReferences.AddRange(assets.Select(o => new SelectReferenceWindow.ObjectReference()
                    {
                        reference = o,
                        source = SelectReferenceWindow.ObjectReference.Source.Asset
                    }));
                    return assetReferences;
                };
                
                SelectReferenceWindow.OpenWindow(new SelectReferenceWindow.Configuration()
                {
                    assetReferencesOpen = options.assetReferencesOpen,
                    prefabReferencesOpen = options.prefabReferencesOpen,
                    sceneReferencesOpen = options.sceneReferencesOpen,
                    getSceneReferences = options.disableSceneReferences ? null : getSceneReferences,
                    getPrefabReferences = options.disablePrefabReferences ? null : getPrefabReferences,
                    getAssetsReferences = options.disableAssetReferences ? null : getAssetReferences,
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
            
            EditorGUI.EndProperty();
        }
        
        private void OnReferenceObjectSelected(SelectReferenceWindow.ObjectReference objectReference)
        {
            lastSelectedObject = objectReference.reference;
        }

    }
    
    [CustomPropertyDrawer(typeof(BaseObjectTypeAttribute), true)]
    public class ObjectTypeAttributePropertyDrawer : PropertyDrawer
    {
        private ObjectTypeAttributeGUI attributeGUI = new();
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
            
            if (objectTypeAttribute == null)
            {
                return;
            }
            
            var typeToSelect = GetTypeToSelect(property);
            var objectProperty = GetPropertyToOverride(property);
            
            attributeGUI.DrawGUI(position, objectProperty, typeToSelect, new ObjectTypeAttributeGUI.Options()
            {
                folders = objectTypeAttribute.GetFolders(),
                filterString = objectTypeAttribute.filterString,
                sceneReferencesFilter = objectTypeAttribute.sceneReferencesFilter,
                sceneReferencesOpen = objectTypeAttribute.sceneReferencesOnWhenStart,
                prefabReferencesOpen = objectTypeAttribute.prefabReferencesOnWhenStart,
                assetReferencesOpen = objectTypeAttribute.assetReferencesOnWhenStart,
                disableSceneReferences = objectTypeAttribute.disableSceneReferences,
                disablePrefabReferences = objectTypeAttribute.disablePrefabReferences,
                disableAssetReferences = objectTypeAttribute.disableAssetReferences
            });
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