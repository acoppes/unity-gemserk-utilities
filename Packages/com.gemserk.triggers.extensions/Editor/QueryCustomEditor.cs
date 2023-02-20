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

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            
            foreach (var type in types)
            {
                if (type.IsAbstract)
                {
                    continue;
                }
                
                var hasParameter = queryParameters
                    .Where(c => c != null)
                    .Count(c => c.GetType() == type) > 0;

                var removed = false;

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"<< {type.Name.Replace("QueryParameter", "")} >>", style);
                
                if (hasParameter)
                {
                    var queryParameter = query.GetComponent(type);
                        
                    removed = GUILayout.Button("Remove");
                        
                    if (removed)
                    {
                        if (EditorUtility.DisplayDialog("Confirm",
                                $"This will remove {queryParameter.GetType().Name} and its serialized data", "Ok", "Cancel"))
                        {
                            GameObject.DestroyImmediate(queryParameter);
                        }
                    }
                }
                else
                {
                    if (GUILayout.Button("Add"))
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
                
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }
            
            EditorGUILayout.EndVertical();
        }
    }
    
}