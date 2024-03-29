﻿using System;
using System.Collections.Generic;
using System.Linq;
using Gemserk.RefactorTools.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gemserk.Utilities.Editor
{
    public class AssetListBaseWindow : EditorWindow
    {
        protected readonly Dictionary<Type, List<Object>> objectsPerType = new();
        protected readonly Dictionary<Type, bool[]> foldoutsPerType = new();
        
        protected readonly List<Type> allowedTypes;
        
        protected readonly Dictionary<Object, UnityEditor.Editor> editorInstances = new();

        protected bool[] typeFoldouts;

        private Vector2 scrollPosition;

        public AssetListBaseWindow(params Type[] types)
        {
            allowedTypes = types.ToList();
            typeFoldouts = new bool[allowedTypes.Count];

            foreach (var type in allowedTypes)
            {
                foldoutsPerType[type] = new bool[100];
            }
        }

        private void OnFocus()
        {
            objectsPerType.Clear();
            editorInstances.Clear();
            
            foreach (var type in allowedTypes)
            {
                objectsPerType[type] = AssetDatabaseExt.FindAssets(type);
                foreach (var obj in objectsPerType[type])
                {
                    editorInstances[obj] = UnityEditor.Editor.CreateEditor(obj);
                }
            }
        }

        private void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, false);
            EditorGUILayout.BeginVertical();

            var multipleTypes = allowedTypes.Count > 1;
            
            for (var j = 0; j < allowedTypes.Count; j++)
            {
                var type = allowedTypes[j];
                var objectsList = objectsPerType[type];
                var foldouts = foldoutsPerType[type];

                var multipleObjects = objectsList.Count > 1;
                
                if (multipleTypes)
                {
                    typeFoldouts[j] = EditorGUILayout.Foldout(typeFoldouts[j], type.Name);
                }
                
                if (!multipleTypes || typeFoldouts[j])
                {
                    if (multipleTypes)
                    {
                        EditorGUI.indentLevel++;
                    }
                    
                    for (var i = 0; i < objectsList.Count; i++)
                    {
                        var asset = objectsList[i];
                        if (asset == null)
                        {
                            continue;
                        }

                        if (multipleObjects)
                        {
                            foldouts[i] = EditorGUILayout.Foldout(foldouts[i], asset.name);
                        }
                        
                        if (!multipleObjects || foldouts[i])
                        {
                            EditorGUI.indentLevel++;
                            
                            // Source asset
                            EditorGUI.BeginDisabledGroup(true);
                            EditorGUILayout.ObjectField(asset, asset.GetType());
                            EditorGUI.EndDisabledGroup();
                            EditorGUILayout.Separator();
                            
                            var objectEditor = editorInstances[asset];
                            
                            objectEditor.OnInspectorGUI();
                            EditorGUI.indentLevel--;
                        }

                    }

                    if (multipleTypes)
                    {
                        EditorGUI.indentLevel--;
                    }
                }
            }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
    }
}