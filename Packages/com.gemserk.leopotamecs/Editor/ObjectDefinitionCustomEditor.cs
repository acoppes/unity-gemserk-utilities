using System;
using System.Collections.Generic;
using System.Linq;
using Gemserk.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Editor
{
    [CustomEditor(typeof(ObjectEntityDefinition), true)]
    public class ObjectDefinitionCustomEditor : UnityEditor.Editor
    {
        private const string k_QueryEditorHideMonobehavioursEditorPref = "Gemserk.QueryEditor.HideMonobehaviours";

        private List<Type> types;
        
        private void OnEnable()
        {
            types = TypeCache.GetTypesDerivedFrom<ComponentDefinitionBase>()
                .Where(t => !t.IsAbstract)
                .ToList();
            types.Sort(delegate(Type a, Type b)
            {
                return string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase); 
            });
        }

        public override void OnInspectorGUI()
        {
            var targetObject = target as ObjectEntityDefinition;

            var hideMonoBehaviours = EditorPrefs.GetBool(k_QueryEditorHideMonobehavioursEditorPref, true);
            hideMonoBehaviours = EditorGUILayout.Toggle("Hide MonoBehaviours", hideMonoBehaviours);
            EditorPrefs.SetBool(k_QueryEditorHideMonobehavioursEditorPref, hideMonoBehaviours);
            
            var style = new GUIStyle(GUI.skin.label);
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = Color.grey;
            
            DrawDefaultInspector();
            
            EditorGUILayout.BeginVertical();
            
            // serializedObject.FindProperty("hideMonoBehaviours")
            
            var components = targetObject
                .GetComponents<ComponentDefinitionBase>().ToList();
            
            foreach (var component in components)
            {
                // componentDefinition.hideFlags = HideFlags.None;

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
                typeNames.AddRange(addTypes.Select(t => t.Name.Replace("Definition", "")));

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
                    EditorGUILayout.LabelField($"<< {type.Name.Replace("Definition", "")} >>", style);
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

            // if (componentDefinitionsFromObjects.Count > 0)
            // {
            //     EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            //
            //     // foreach component render inside this one + remove button
            //
            //
            //
            //     if (hideMonoBehaviours)
            //     {
            //         // EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            //
            //         componentDefinitionsFromObjects.Sort(delegate(ComponentDefinitionBase a, ComponentDefinitionBase b)
            //         {
            //             return string.Compare(a.GetType().Name, b.GetType().Name,
            //                 StringComparison.OrdinalIgnoreCase);
            //         });
            //
            //         var copyOfDefinitions = new List<ComponentDefinitionBase>(componentDefinitionsFromObjects);
            //         foreach (var componentDefinition in copyOfDefinitions)
            //         {
            //             EditorGUILayout.BeginHorizontal();
            //             EditorGUILayout.LabelField($"<< {componentDefinition.GetComponentName()} >>", style);
            //
            //             var remove = GUILayout.Button("Remove");
            //             
            //             if (remove)
            //             {
            //                 if (EditorUtility.DisplayDialog("Confirm",
            //                         "This will remove the component and lose serialized data with it", "Ok", "Cancel"))
            //                 {
            //                     GameObject.DestroyImmediate(componentDefinition);
            //                 }
            //             }
            //
            //             EditorGUILayout.EndHorizontal();
            //
            //             if (!remove)
            //             {
            //                 var serializedObject = new SerializedObject(componentDefinition);
            //                 CustomEditorExtensions.DrawInspectorExcept(serializedObject, new[] { "m_Script" });
            //                 // EditorGUILayout.Separator();
            //                 EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            //             }
            //
            //         }
            //     }
            // }
            
            EditorGUILayout.EndVertical();
        }
    }
}