using System;
using System.Collections.Generic;
using System.Linq;
using Gemserk.RefactorTools.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gemserk.Utilities.Editor
{
    public class TempWindowTest : EditorWindow
    {
        public static void OpenWindow(IEnumerable<Object> objects, Action<Object> onSelected = null)
        {
            var window = GetWindow<TempWindowTest>();
            window.titleContent = new GUIContent("Select Reference");
            window.objects.Clear();
            window.objects.AddRange(objects);
            // window.onClose = onClose;
            window.onSelected = onSelected;
        }
        
        private List<Object> objects = new List<Object>();
        
        private Action<Object> onSelected;
        // private Action onClose;
        
        // [MenuItem("Window/Gemserk/All Things Window")]

        private Vector2 scrollPosition;
        private string searchText = "";
        
        private void OnGUI()
        {
            string[] searchTexts = null;
            
            EditorGUILayout.BeginHorizontal();
            searchText = EditorGUILayout.TextField("Search", searchText);
            EditorGUILayout.EndHorizontal();

            if (!string.IsNullOrEmpty(searchText))
            {
                searchText = searchText.TrimStart().TrimEnd();
                if (!string.IsNullOrEmpty(searchText))
                {
                    searchTexts = searchText.Split(' ');
                }
            }
            
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            foreach (var obj in objects)
            {
                var match = true;
                
                if (searchTexts != null && searchTexts.Length > 0)
                {
                    foreach (var text in searchTexts)
                    {
                        if (!obj.name.Contains(text, StringComparison.OrdinalIgnoreCase))
                        {
                            match = false;
                            break;
                        }
                    }
                }

                if (!match)
                {
                    continue;
                }
                
                // if there is a path, show wpath
                EditorGUILayout.BeginHorizontal();
                
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.ObjectField(obj, obj.GetType(), false);
                EditorGUI.EndDisabledGroup();
                
                // EditorGUILayout.LabelField(obj.name);
                // EditorGUILayout.LabelField(obj.GetType().Name);
                
                if (GUILayout.Button("Select"))
                {
                    onSelected.Invoke(obj);
                    Close();
                }
                
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }
    }
    
    [CustomPropertyDrawer(typeof(ObjectTypeAttribute), true)]
    public class ObjectTypeAttributePropertyDrawer : PropertyDrawer
    {
        private const float elementHeight = 18f;
        private List<Object> options = new List<Object>();

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

            var buttonPosition = new Rect(position.x, position.y, position.width, elementHeight);
          //  var optionsPosition = new Rect(position.x, position.y + elementHeight, position.width, elementHeight);
            var objectPosition = new Rect(position.x, position.y + elementHeight * 1, position.width, elementHeight);

            if (lastSelectedObject != null)
            {
                property.objectReferenceValue = lastSelectedObject;
                lastSelectedObject = null;
            }
            
            if (GUI.Button(buttonPosition, "Select"))
            {
                options.Clear();

                // var sceneObjects = GameObject.FindObjectsByType(typeToSelect, FindObjectsInactive.Include, FindObjectsSortMode.None);
                // options.AddRange(sceneObjects);
                
                var prefabsWithType = AssetDatabaseExt.FindPrefabs(new []{typeToSelect}, AssetDatabaseExt.FindOptions.ConsiderInactiveChildren, new []
                {
                    "Assets"
                });
                
                options.AddRange(prefabsWithType);

                var assets = AssetDatabaseExt.FindAssetsAll(typeToSelect, null, new[] { "Assets" });
                options.AddRange(assets);
                
                TempWindowTest.OpenWindow(options, o =>
                {
                    lastSelectedObject = o;
                });
            }

            // if (options.Count > 0)
            // {
            //     var selectedIndex = options.IndexOf(property.objectReferenceValue);
            //     var newIndex = EditorGUI.Popup(optionsPosition, selectedIndex, options.Select(o => $"{AssetDatabase.GetAssetPath(o)}").ToArray());
            //     
            //     if (newIndex >= 0 && newIndex < options.Count)
            //     {
            //         property.objectReferenceValue = options[newIndex];
            //     }
            // }
            // else
            // {
            //     EditorGUI.LabelField(optionsPosition, "Not loaded");
            // }

            EditorGUI.ObjectField(objectPosition, property);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * 2; 
        }
    }
}