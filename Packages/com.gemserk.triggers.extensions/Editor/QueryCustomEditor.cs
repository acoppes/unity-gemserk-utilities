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
        private const string k_QueryEditorHideMonobehavioursEditorPref = "Gemserk.QueryEditor.HideMonobehaviours";
        
        private List<Type> types;
        
        private void OnEnable()
        {
            types = TypeCache.GetTypesDerivedFrom<QueryParameterBase>()
                .Where(t => !t.IsAbstract)
                .ToList();
            types.Sort(delegate(Type a, Type b)
            {
                return string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase); 
            });
        }

        public override void OnInspectorGUI()
        {
            var targetObject = target as Query;
            
            var hideMonoBehaviours = EditorPrefs.GetBool(k_QueryEditorHideMonobehavioursEditorPref, true);
            hideMonoBehaviours = EditorGUILayout.Toggle("Hide MonoBehaviours", hideMonoBehaviours);
            EditorPrefs.SetBool(k_QueryEditorHideMonobehavioursEditorPref, hideMonoBehaviours);
            
            // fix object name
            targetObject.gameObject.name = $"Q({targetObject.GetEntityQuery()})";
            
            var style = new GUIStyle(GUI.skin.label);
            style.alignment = TextAnchor.MiddleCenter;
            
            DrawDefaultInspector();
            
            EditorGUILayout.BeginVertical();
            
            var components = targetObject
                .GetComponents<QueryParameterBase>().ToList();
            
            foreach (var component in components)
            {
                if (hideMonoBehaviours)
                {
                    component.hideFlags = HideFlags.HideInInspector;
                }
                else
                {
                    component.hideFlags = HideFlags.None;
                }
            }

            var buttons = 0;
            
            var addedTypes = components.Select(c => c.GetType())
                .ToList();
            addedTypes.Sort(delegate(Type a, Type b)
            {
                return string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase); 
            });
            
            var addTypes = types.Except(addedTypes).ToList();

            if (addTypes.Count > 0)
            {
                var typeNames = new List<string>(new[] { "<< SELECT TO ADD >>" });
                typeNames.AddRange(addTypes.Select(t => t.Name.Replace("QueryParameter", "")));

                var selected = 0;
                EditorGUI.BeginChangeCheck();
                selected = EditorGUILayout.Popup(selected, typeNames.ToArray());
                if (EditorGUI.EndChangeCheck())
                {
                    var typeToAdd = addTypes[selected - 1];
                    targetObject.gameObject.AddComponent(typeToAdd);
                    EditorUtility.SetDirty(targetObject);
                    AssetDatabase.SaveAssetIfDirty(targetObject);
                }
            }
            else
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.Popup(0, new []{ "<< NO ELEMENTS TO ADD >>"});
                EditorGUI.EndDisabledGroup();
            }

            foreach (var type in addedTypes)
            {
                var hasComponentOfType = components
                    .Where(c => c != null)
                    .Count(c => c.GetType() == type) > 0;

                var removed = false;

                var showCustomEditor = !hasComponentOfType || hideMonoBehaviours;
                
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
                    if (hasComponentOfType)
                    {
                        style.normal.textColor = Color.white;
                    }
                    else
                    {
                        style.normal.textColor = Color.grey;
                    }
                    EditorGUILayout.LabelField($"<< {type.Name.Replace("QueryParameter", "")} >>", style);
                }

                if (hasComponentOfType)
                {
                    if (hideMonoBehaviours)
                    {
                        var component = targetObject.GetComponent(type);

                        removed = GUILayout.Button("-", GUILayout.MaxWidth(20));

                        if (removed)
                        {
                            if (EditorUtility.DisplayDialog("Confirm",
                                    $"This will remove {component.GetType().Name} and its serialized data", "Ok",
                                    "Cancel"))
                            {
                                GameObject.DestroyImmediate(component);
                                EditorUtility.SetDirty(targetObject);
                                AssetDatabase.SaveAssetIfDirty(targetObject);
                            }
                        }
                    }
                }
                else
                {
                    if (GUILayout.Button("+", GUILayout.MaxWidth(20)))
                    {
                        targetObject.gameObject.AddComponent(type);
                        EditorUtility.SetDirty(targetObject);
                        AssetDatabase.SaveAssetIfDirty(targetObject);
                    }
                }

                EditorGUILayout.EndHorizontal();

                if (hideMonoBehaviours && hasComponentOfType && !removed)
                {
                    var component = targetObject.GetComponent(type);
                    var serializedObject = new SerializedObject(component);
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