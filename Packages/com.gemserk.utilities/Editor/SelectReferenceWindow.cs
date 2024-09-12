using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gemserk.Utilities.Editor
{
    public class SelectReferenceWindow : EditorWindow
    {
        public struct Configuration
        {
            public bool sceneReferencesOpen;
            public bool prefabReferencesOpen;
            public bool assetReferencesOpen;
            
            public Func<List<ObjectReference>> getSceneReferences;
            public Func<List<ObjectReference>> getPrefabReferences;
            public Func<List<ObjectReference>> getAssetsReferences;

            public SerializedProperty property;
        }

        public struct ObjectReference
        {
            public enum Source
            {
                Scene, Prefab, Asset
            }

            public Source source;
            public Object reference;
            public bool enabled;
        }
        
        public static void OpenWindow(Configuration configuration)
        {
            var window = GetWindow<SelectReferenceWindow>();
            window.titleContent = new GUIContent("Select Reference");
            window.objects.Clear();
            // window.objects.AddRange(objects);
            window.configuration = configuration;
            
            window.numToggles = 0;

            if (configuration.getSceneReferences != null)
            {
                window.numToggles++;
            }
            
            if (configuration.getPrefabReferences != null)
            {
                window.numToggles++;
            }
            
            if (configuration.getAssetsReferences != null)
            {
                window.numToggles++;
            }
            
            window.showSceneReferences = configuration.sceneReferencesOpen || window.numToggles <= 1;
            window.showPrefabReferences = configuration.prefabReferencesOpen || window.numToggles <= 1;
            window.showAssetsReferences = configuration.assetReferencesOpen || window.numToggles <= 1;
            
            window.RecalculateObjects();
        }

        private Configuration configuration;
        
        private List<ObjectReference> objects = new List<ObjectReference>();
        
        private int numToggles = 0;
        
        // private Action onClose;
        
        // [MenuItem("Window/Gemserk/All Things Window")]

        private Vector2 scrollPosition;
        private string searchText = "";

        private bool showSceneReferences = true;
        private bool showAssetsReferences = true;
        private bool showPrefabReferences = true;

        private SearchField searchField;

        private bool hideDisabledObjects;
        // private bool filtersFoldout = true;

        private void RecalculateObjects()
        {
            objects.Clear();
            
            if (showSceneReferences&& configuration.getSceneReferences != null)
            {
                var sceneReferences = configuration.getSceneReferences();
                
                if (hideDisabledObjects)
                {
                    objects.AddRange(sceneReferences.Where(o => o.enabled).ToArray());  
                }
                else
                {
                    objects.AddRange(sceneReferences);    
                }
                
            }
            
            if (showPrefabReferences && configuration.getPrefabReferences != null)
            {
                objects.AddRange(configuration.getPrefabReferences());    
            }
            
            if (showAssetsReferences && configuration.getAssetsReferences != null)
            {
                objects.AddRange(configuration.getAssetsReferences());    
            }
        }
        
        private void OnGUI()
        {
            // var enabledSceneObjectIcon =  EditorGUIUtility.IconContent("GameObject On Icon");
            // var disabledSceneObjectIcon = EditorGUIUtility.IconContent("GameObject Icon");
            // var prefabIcon = EditorGUIUtility.IconContent("Prefab Icon");
            // var asseteIcon = EditorGUIUtility.IconContent("ScriptableObject Icon");
            
            string[] searchTexts = null;
             
            if (searchField == null)
            {
                searchField = new SearchField();
            }

            var rect = EditorGUILayout.GetControlRect();
            searchText = searchField.OnGUI(rect, searchText);

            var optionsChanged = false;
            
            if (numToggles > 1)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();
                
                if (configuration.getSceneReferences != null)
                {
                    showSceneReferences = GUILayout.Toggle(showSceneReferences, "Scene", "Button");
                }
                
                if (configuration.getPrefabReferences != null)
                {
                    showPrefabReferences = GUILayout.Toggle(showPrefabReferences, "Prefabs", "Button");
                }
                
                if (configuration.getAssetsReferences != null)
                {
                    showAssetsReferences = GUILayout.Toggle(showAssetsReferences, "Assets", "Button");
                }
                
                optionsChanged = EditorGUI.EndChangeCheck();
                EditorGUILayout.EndHorizontal();
            }
            
            if (!string.IsNullOrEmpty(searchText))
            {
                searchText = searchText.TrimStart().TrimEnd();
                if (!string.IsNullOrEmpty(searchText))
                {
                    searchTexts = searchText.Split(' ');
                }
            }

            // filtersFoldout = EditorGUILayout.Foldout(filtersFoldout, new GUIContent("Filters"));
            // if (filtersFoldout)
            // {

            if (showSceneReferences)
            {
                EditorGUI.BeginChangeCheck();
                hideDisabledObjects = GUILayout.Toggle(hideDisabledObjects, "Hide Inactive GameObjects", "Button");
                optionsChanged = optionsChanged || EditorGUI.EndChangeCheck();
            }
            
            // }
            
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            foreach (var objectReference in objects)
            {
                var match = true;

                if (objectReference.source == ObjectReference.Source.Scene && !showSceneReferences)
                {
                    continue;
                }
                
                if (objectReference.source == ObjectReference.Source.Prefab && !showPrefabReferences)
                {
                    continue;
                }
                
                if (objectReference.source == ObjectReference.Source.Asset && !showAssetsReferences)
                {
                    continue;
                }
                
                if (searchTexts != null && searchTexts.Length > 0)
                {
                    foreach (var text in searchTexts)
                    {
                        if (!objectReference.reference.name.Contains(text, StringComparison.OrdinalIgnoreCase))
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
                EditorGUILayout.ObjectField(objectReference.reference, objectReference.reference.GetType(), true);
                EditorGUI.EndDisabledGroup();
                
                // Show icon per type of reference
                // if (objectReference.source == ObjectReference.Source.Scene)
                // {
                //     // EditorGUILayout.LabelField(objectReference.enabled ? new GUIContent(enabledSceneObjectIcon) : new GUIContent(disabledSceneObjectIcon), 
                //     //     GUILayout.MaxWidth(20));
                //     EditorGUILayout.LabelField(new GUIContent(), 
                //         GUILayout.MaxWidth(20), GUILayout.MinWidth(20));
                // } else if (objectReference.source == ObjectReference.Source.Prefab)
                // {
                //     EditorGUILayout.LabelField(new GUIContent(prefabIcon), GUILayout.MaxWidth(20));
                // } else if (objectReference.source == ObjectReference.Source.Asset)
                // {
                //     EditorGUILayout.LabelField(new GUIContent(asseteIcon), GUILayout.MaxWidth(20));
                // }
                
                // EditorGUILayout.LabelField(obj.name);
                // EditorGUILayout.LabelField(obj.GetType().Name);
                
                if (GUILayout.Button("Select"))
                {
                    configuration.property.objectReferenceValue = objectReference.reference;
                    configuration.property.serializedObject.ApplyModifiedProperties();
                    // configuration.onSelectReferenceCallback.Invoke(objectReference);
                    Close();
                }
                
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
            
            if (GUILayout.Button("Close"))
            {
                Close();
            }

            if (optionsChanged)
            {
                RecalculateObjects();
                Repaint();
            }
        }
    }
}