using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gemserk.Utilities.Editor
{
    public class SelectReferenceWindow : EditorWindow
    {
        public static void OpenWindow(IEnumerable<Object> objects, System.Action<Object> onSelected = null)
        {
            var window = GetWindow<SelectReferenceWindow>();
            window.titleContent = new GUIContent("Select Reference");
            window.objects.Clear();
            window.objects.AddRange(objects);
            // window.onClose = onClose;
            window.onSelected = onSelected;
        }
        
        private List<Object> objects = new List<Object>();
        
        private System.Action<Object> onSelected;
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
                if (obj == null)
                {
                    Debug.LogError("obj null");
                    continue;
                }
                
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
                EditorGUILayout.ObjectField(obj, obj.GetType(), true);
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
            
            if (GUILayout.Button("Close"))
            {
                Close();
            }
        }
    }
}