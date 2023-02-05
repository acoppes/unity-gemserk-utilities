using System;
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
        protected readonly List<Type> allowedTypes;

        protected bool[] typeFoldouts;

        private Vector2 scrollPosition;

        public AssetListBaseWindow(params Type[] types)
        {
            allowedTypes = types.ToList();
            typeFoldouts = new bool[allowedTypes.Count];
        }

        private void OnFocus()
        {
            objectsPerType.Clear();
            foreach (var type in allowedTypes)
            {
                objectsPerType[type] = AssetDatabaseExt.FindAssets(type);
            }
        }

        private void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, false);
            EditorGUILayout.BeginVertical();

            var multiple = allowedTypes.Count > 1;
            
            for (var j = 0; j < allowedTypes.Count; j++)
            {
                var type = allowedTypes[j];
                var objectsList = objectsPerType[type];
                
                if (multiple)
                {
                    typeFoldouts[j] = EditorGUILayout.Foldout(typeFoldouts[j], type.Name);
                }
                
                if (!multiple || typeFoldouts[j])
                {
                    if (multiple)
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

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(asset.name);
                        if (GUILayout.Button("Select"))
                        {
                            Selection.activeObject = asset;
                        }

                        EditorGUILayout.EndHorizontal();
                    }

                    if (multiple)
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