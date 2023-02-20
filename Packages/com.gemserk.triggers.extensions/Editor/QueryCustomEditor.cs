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
            
            var style = new GUIStyle(GUI.skin.label);
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = Color.grey;
            
            EditorGUILayout.BeginVertical();
            
            // serializedObject.FindProperty("hideMonoBehaviours")
            
            var queryParameters = query
                .GetComponentsInChildren<QueryParameterBase>().ToList();

            var buttons = 0;

            foreach (var type in types)
            {
                if (type.IsAbstract)
                {
                    continue;
                }
                
                var hasParameter = queryParameters
                    .Where(c => c != null)
                    .Count(c => c.GetType() == type) > 0;

                if (hasParameter)
                {
                    continue;
                }

                if (buttons == 0)
                {
                    EditorGUILayout.LabelField($"<< Add >>", style);
                }

                buttons++;

                if (!hasParameter && GUILayout.Button($"{type.Name.Replace("QueryParameter", "")}"))
                {
                    query.gameObject.AddComponent(type);
                    EditorUtility.SetDirty(query);
                    AssetDatabase.SaveAssetIfDirty(query);
                }
            }

            if (buttons > 0)
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }
            
            DrawDefaultInspector();
            
            if (queryParameters.Count > 0)
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                // foreach component render inside this one + remove button

                foreach (var queryParameter in queryParameters)
                {
                    // componentDefinition.hideFlags = HideFlags.None;
                
                    if (query.hideMonoBehaviours)
                    {
                        queryParameter.hideFlags = HideFlags.HideInInspector;
                    }
                    else
                    {
                        queryParameter.hideFlags = HideFlags.None;
                    }
                }
                
                if (query.hideMonoBehaviours)
                {
                    // EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                
                    queryParameters.Sort(delegate(QueryParameterBase a, QueryParameterBase b)
                    {
                        return string.Compare(a.GetType().Name, b.GetType().Name,
                            StringComparison.OrdinalIgnoreCase);
                    });
                
                    // var copyOfDefinitions = new List<QueryParameterBase>(queryParameters);
                    
                    foreach (var queryParameter in queryParameters)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField($"<< {queryParameter.GetType().Name.Replace("QueryParameter", "")} >>", style);
                
                        var remove = GUILayout.Button("Remove");
                        
                        if (remove)
                        {
                            if (EditorUtility.DisplayDialog("Confirm",
                                    $"This will remove {queryParameter.GetType().Name} and its serialized data", "Ok", "Cancel"))
                            {
                                GameObject.DestroyImmediate(queryParameter);
                            }
                            
                           
                        }
                
                        EditorGUILayout.EndHorizontal();
                
                        if (!remove)
                        {
                            var serializedObject = new SerializedObject(queryParameter);
                            CustomEditorExtensions.DrawInspectorExcept(serializedObject, new[] { "m_Script" });
                            // EditorGUILayout.Separator();
                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                        }
                
                    }
                }
            }

            query.gameObject.name = $"Q({query.GetEntityQuery()})";
            
            EditorGUILayout.EndVertical();
        }
    }
    
}