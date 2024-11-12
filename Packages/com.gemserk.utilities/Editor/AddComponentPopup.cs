﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Gemserk.Utilities.Editor
{
    public class AddComponentPopup : PopupWindowContent
    {
        public List<Type> types;
        public SerializedObject serializedObject;
        public string[] cleanupFilter = null;

        private Vector2 scroll;
        
        private SearchField searchField;
        private string searchText;
        
        public override void OnGUI(Rect rect)
        {
            var renamedTypes = new List<Type>(types);
            
            var buttonSkin = new GUIStyle(GUI.skin.button);
            buttonSkin.alignment = TextAnchor.MiddleLeft;
            buttonSkin.normal.textColor = new Color(1, 1, 1, 1f);
            var buttonHeight = buttonSkin.CalcHeight(new GUIContent("Example"), rect.width) + 2.5f;
            
            var withComponentSkin = new GUIStyle(buttonSkin);
            withComponentSkin.normal.textColor = new Color(0.8f, 0.8f, 0.8f, 1f);
            
            var component = serializedObject.targetObject as Component;
            
            if (searchField == null)
            {
                searchField = new SearchField();
            }
            
            string[] searchTexts = null;
            searchText = searchField.OnToolbarGUI(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), searchText);
            if (!string.IsNullOrEmpty(searchText))
            {
                searchText = searchText.TrimStart().TrimEnd();
                if (!string.IsNullOrEmpty(searchText))
                {
                    searchTexts = searchText.Split(' ');
                }
            }

            if (searchTexts != null && searchTexts.Length > 0)
            {
                renamedTypes = renamedTypes.Where(r =>
                {
                    foreach (var text in searchTexts)
                    {
                        if (!r.Name.Contains(text, StringComparison.OrdinalIgnoreCase))
                        {
                            return false;
                        }
                    }

                    return true;
                }).ToList();
            }

            scroll = GUI.BeginScrollView(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight, rect.width, rect.height - EditorGUIUtility.singleLineHeight), scroll,
                new Rect(0, 0, rect.width - 15, buttonHeight * (renamedTypes.Count)), 
                false, true);
            
            // scroll = EditorGUILayout.BeginScrollView(scroll, false, true);
            foreach (var type in renamedTypes)
            {
                var name = type.Name;
                
                if (cleanupFilter != null)
                {
                    foreach (var filter in cleanupFilter)
                    {
                        name = name.Replace(filter, "");
                    }
                }

                var number = component.GetComponents(type).Length;
                
                if (GUILayout.Button(number > 0 ? $"{name} ({number})" : name, number > 0 ? withComponentSkin :buttonSkin))
                {
                    Debug.Log($"Component Added: {name}");
                    GuiUtilities.AddComponentWithUndo(serializedObject, type);
                    
                    editorWindow.Close();
                }
            }
            GUI.EndScrollView();
        }

        public override Vector2 GetWindowSize()
        {
            // var windowSize = base.GetWindowSize();
            return new Vector2(300f, Mathf.Min(400f, EditorGUIUtility.singleLineHeight * (types.Count + 3)));
        }
    }
}