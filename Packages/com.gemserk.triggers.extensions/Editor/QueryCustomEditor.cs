using System;
using System.Collections.Generic;
using System.Linq;
using Gemserk.Triggers.Queries;
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
            types = TypeCache.GetTypesDerivedFrom<IQueryParameter>().ToList();
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
                .GetComponentsInChildren<IQueryParameter>().ToList();

            var buttons = 0;
            
            EditorGUILayout.LabelField($"<< Add >>", style);

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

                // foreach (var componentDefinition in queryParameters)
                // {
                //     // componentDefinition.hideFlags = HideFlags.None;
                //
                //     if (query.hideMonoBehaviours)
                //     {
                //         componentDefinition.hideFlags = HideFlags.HideInInspector;
                //     }
                //     else
                //     {
                //         componentDefinition.hideFlags = HideFlags.None;
                //     }
                // }
                //
                // if (query.hideMonoBehaviours)
                // {
                //     // EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                //
                //     componentDefinitionsFromObjects.Sort(delegate(ComponentDefinitionBase a, ComponentDefinitionBase b)
                //     {
                //         return string.Compare(a.GetType().Name, b.GetType().Name,
                //             StringComparison.OrdinalIgnoreCase);
                //     });
                //
                //     var copyOfDefinitions = new List<ComponentDefinitionBase>(componentDefinitionsFromObjects);
                //     foreach (var componentDefinition in copyOfDefinitions)
                //     {
                //         EditorGUILayout.BeginHorizontal();
                //         EditorGUILayout.LabelField($"<< {componentDefinition.GetComponentName()} >>", style);
                //
                //         var remove = GUILayout.Button("Remove");
                //         
                //         if (remove)
                //         {
                //             if (EditorUtility.DisplayDialog("Confirm",
                //                     "This will remove the component and lose serialized data with it", "Ok", "Cancel"))
                //             {
                //                 GameObject.DestroyImmediate(componentDefinition);
                //             }
                //         }
                //
                //         EditorGUILayout.EndHorizontal();
                //
                //         if (!remove)
                //         {
                //             var serializedObject = new SerializedObject(componentDefinition);
                //             CustomEditorExtensions.DrawInspectorExcept(serializedObject, new[] { "m_Script" });
                //             // EditorGUILayout.Separator();
                //             EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                //         }
                //
                //     }
                // }
            }
            
            EditorGUILayout.EndVertical();
        }
    }
    
}