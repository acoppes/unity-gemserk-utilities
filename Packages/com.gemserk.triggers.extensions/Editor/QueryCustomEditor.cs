using System;
using System.Collections.Generic;
using System.Linq;
using Gemserk.Triggers.Queries;
using Gemserk.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Triggers.Editor
{
    [CustomEditor(typeof(Query), true)]
    public class QueryCustomEditor : UnityEditor.Editor
    {
        private List<Type> types;
        
        private void OnEnable()
        {
            types = TypeCache.GetTypesDerivedFrom<QueryParameterBase>().ToList();
        }

        public override void OnInspectorGUI()
        {
            var query = target as Query;

            if (query == null)
            {
                return;
            }
            
            // fix object name
            query.gameObject.name = $"Q({query.GetEntityQuery()})";
            
            var style = new GUIStyle(GUI.skin.label);
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = Color.grey;
            
            DrawDefaultInspector();
            
            EditorGUILayout.BeginVertical();
            
            // serializedObject.FindProperty("hideMonoBehaviours")
            
            var queryParameters = query
                .GetComponentsInChildren<QueryParameterBase>().ToList();
            
            foreach (var queryParameter in queryParameters)
            {
                if (query.hideMonoBehaviours)
                {
                    queryParameter.hideFlags = HideFlags.HideInInspector;
                }
                else
                {
                    queryParameter.hideFlags = HideFlags.None;
                }
            }

            var buttons = 0;
            
            foreach (var type in types)
            {
                if (type.IsAbstract)
                {
                    continue;
                }
                
                var type1 = type;
                var hasParameter = queryParameters
                    .Where(c => c != null)
                    .Count(c => c.GetType() == type1) > 0;

                var removed = false;

                var showCustomEditor = !hasParameter || query.hideMonoBehaviours;
                
                if (showCustomEditor)
                {
                    if (buttons == 0)
                    {
                        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    }
                }
                
                EditorGUILayout.BeginHorizontal();

                if (showCustomEditor)
                {
                    EditorGUILayout.LabelField($"<< {type.Name.Replace("QueryParameter", "")} >>", style);
                }

                if (hasParameter)
                {
                    if (query.hideMonoBehaviours)
                    {
                        var queryParameter = query.GetComponent(type);

                        removed = GUILayout.Button("-", GUILayout.MaxWidth(20));

                        if (removed)
                        {
                            if (EditorUtility.DisplayDialog("Confirm",
                                    $"This will remove {queryParameter.GetType().Name} and its serialized data", "Ok",
                                    "Cancel"))
                            {
                                GameObject.DestroyImmediate(queryParameter);
                            }
                        }
                    }
                }
                else
                {
                    if (GUILayout.Button("+", GUILayout.MaxWidth(20)))
                    {
                        query.gameObject.AddComponent(type);
                        EditorUtility.SetDirty(query);
                        AssetDatabase.SaveAssetIfDirty(query);
                    }
                }

                EditorGUILayout.EndHorizontal();

                if (query.hideMonoBehaviours && hasParameter && !removed)
                {
                    var queryParameter = query.GetComponent(type);
                    var serializedObject = new SerializedObject(queryParameter);
                    CustomEditorExtensions.DrawInspectorExcept(serializedObject, new[] { "m_Script" });
                }

                if (showCustomEditor)
                {
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                }

                buttons++;
            }

            EditorGUILayout.EndVertical();
        }
    }
    
}