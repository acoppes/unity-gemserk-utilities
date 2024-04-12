using System.Collections.Generic;
using System.Linq;
using Gemserk.RefactorTools.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gemserk.Utilities.Editor
{
    [CustomPropertyDrawer(typeof(ObjectTypeAttribute), true)]
    public class ObjectTypeAttributePropertyDrawer : PropertyDrawer
    {
        private const float elementHeight = 18f;
        private List<SelectReferenceWindow.ObjectReference> options = new List<SelectReferenceWindow.ObjectReference>();

        private Object lastSelectedObject;
        
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

            var buttonPosition = new Rect(position.x + position.width * 0.25f, position.y, position.width * 0.75f, elementHeight);
            var labelPosition = new Rect(position.x, position.y, position.width * 0.25f, elementHeight * 2);
          //  var optionsPosition = new Rect(position.x, position.y + elementHeight, position.width, elementHeight);
            var objectPosition = new Rect(position.x + position.width * 0.25f, position.y + elementHeight * 1, position.width * 0.75f, elementHeight);

            if (lastSelectedObject != null)
            {
                property.objectReferenceValue = lastSelectedObject;
                lastSelectedObject = null;
            }
            
            EditorGUI.LabelField(labelPosition, property.displayName);
            
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

            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.ObjectField(objectPosition, property, GUIContent.none);
            EditorGUI.EndDisabledGroup();
        }

        private void OnReferenceObjectSelected(SelectReferenceWindow.ObjectReference objectReference)
        {
            lastSelectedObject = objectReference.reference;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * 2; 
        }
    }
}