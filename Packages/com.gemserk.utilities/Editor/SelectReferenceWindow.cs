using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gemserk.Utilities.Editor
{
    public class SelectReferenceWindow : EditorWindow
    {
        public struct Configuration
        {
            public bool canSelectSceneReferences;
            public bool canSelectAssetReferences;
            public bool canSelectPrefabReferences;

            public Action<ObjectReference> onSelectReferenceCallback;
        }

        public struct ObjectReference
        {
            public enum Source
            {
                Scene, Prefab, Asset
            }

            public Source source;
            public Object reference;
        }
        
        public static void OpenWindow(IEnumerable<ObjectReference> objects, Configuration configuration)
        {
            var window = GetWindow<SelectReferenceWindow>();
            window.titleContent = new GUIContent("Select Reference");
            window.objects.Clear();
            window.objects.AddRange(objects);
            window.configuration = configuration;
        }

        private Configuration configuration;
        
        private List<ObjectReference> objects = new List<ObjectReference>();
        
        // private Action onClose;
        
        // [MenuItem("Window/Gemserk/All Things Window")]

        private Vector2 scrollPosition;
        private string searchText = "";

        private bool showSceneReferences = true;
        private bool showAssetsReferences = true;
        private bool showPrefabReferences = true;
        
        private void OnGUI()
        {
            string[] searchTexts = null;
            
            EditorGUILayout.BeginHorizontal();
            searchText = EditorGUILayout.TextField("Search", searchText);
            EditorGUILayout.EndHorizontal();

            var toggleOptionsCount = 0;
            toggleOptionsCount += configuration.canSelectAssetReferences ? 1 : 0;
            toggleOptionsCount += configuration.canSelectPrefabReferences ? 1 : 0;
            toggleOptionsCount += configuration.canSelectSceneReferences ? 1 : 0;
            
            if (toggleOptionsCount > 1)
            {
                EditorGUILayout.BeginHorizontal();
                if (configuration.canSelectSceneReferences)
                {
                    showSceneReferences = GUILayout.Toggle(showSceneReferences, "Scene", "Button");
                }
                if (configuration.canSelectPrefabReferences)
                {
                    showPrefabReferences = GUILayout.Toggle(showPrefabReferences, "Prefabs", "Button");
                }
                if (configuration.canSelectAssetReferences)
                {
                    showAssetsReferences = GUILayout.Toggle(showAssetsReferences, "Assets", "Button");
                }
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
                
                // EditorGUILayout.LabelField(obj.name);
                // EditorGUILayout.LabelField(obj.GetType().Name);
                
                if (GUILayout.Button("Select"))
                {
                    configuration.onSelectReferenceCallback.Invoke(objectReference);
                    Close();
                }
                
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
            
            if (GUILayout.Button("Close"))
            {
                Close();
            }
        }
    }
}