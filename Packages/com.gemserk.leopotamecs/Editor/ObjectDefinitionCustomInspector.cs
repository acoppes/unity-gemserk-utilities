using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Editor
{
    [CustomEditor(typeof(ObjectEntityDefinition), true)]
    public class ObjectDefinitionCustomInspector : UnityEditor.Editor
    {
        // private List<Type> entityComponentDefinitionTypes;
        private List<Type> entityComponentDefinitionObjectsTypes;
        
        private void OnEnable()
        {
            // entityComponentDefinitionTypes = TypeCache.GetTypesDerivedFrom<ComponentDefinitionBase>().ToList();
            entityComponentDefinitionObjectsTypes = TypeCache.GetTypesDerivedFrom<ComponentDefinitionBase>().ToList();
        }

        public override void OnInspectorGUI()
        {
            // base.OnInspectorGUI();

            var objectEntityDefinition = target as ObjectEntityDefinition;

            // var componentDefinitions = objectEntityDefinition.componentDefinitions;
            //
            // foreach (var componentDefinition in componentDefinitions)
            // {
            //     
            // }

            // add buttons for each kind of ientitydefinitions serializable
            
            // var count = 0;
            //
            // foreach (var type in entityComponentDefinitionTypes)
            // {
            //     if (objectEntityDefinition.componentDefinitions
            //             .Where(c => c != null)
            //             .Count(c => c.GetType() == type) > 0)
            //     {
            //         continue;
            //     }
            //
            //     if (type.IsAbstract)
            //     {
            //         continue;
            //     }
            //
            //     count++;
            //     
            //     if (GUILayout.Button($"Add {type.Name.Replace("Definition", "")}"))
            //     {
            //         var componentDefinition = (IComponentDefinition) Activator.CreateInstance(type);
            //         objectEntityDefinition.componentDefinitions.Add(componentDefinition);
            //         EditorUtility.SetDirty(objectEntityDefinition);
            //         AssetDatabase.SaveAssetIfDirty(objectEntityDefinition);
            //         
            //     }
            // }
            //
            // if (count == 0)
            // {
            //     GUILayout.Label("No more components from serialized reference to add");
            // }
            //
            // EditorGUILayout.Separator();

            if (objectEntityDefinition == null)
            {
                return;
            }
            
            var style = new GUIStyle(GUI.skin.label);
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = Color.grey;
            
            EditorGUILayout.BeginVertical();
            
            // serializedObject.FindProperty("hideMonoBehaviours")
            
            var componentDefinitionsFromObjects = objectEntityDefinition
                .GetComponentsInChildren<ComponentDefinitionBase>().ToList();

            var buttons = 0;

            foreach (var type in entityComponentDefinitionObjectsTypes)
            {
                if (type.IsAbstract)
                {
                    continue;
                }
                
                var hasComponent = componentDefinitionsFromObjects
                    .Where(c => c != null)
                    .Count(c => c.GetType() == type) > 0;

                if (hasComponent)
                {
                    continue;
                }

                buttons++;

                if (!hasComponent && GUILayout.Button($"Add {type.Name.Replace("Definition", "")}"))
                {
                    // var gameObject = new GameObject(type.Name);
                    // gameObject.transform.SetParent(objectEntityDefinition.transform);
                    objectEntityDefinition.gameObject.AddComponent(type);
                    
                    EditorUtility.SetDirty(objectEntityDefinition);
                    AssetDatabase.SaveAssetIfDirty(objectEntityDefinition);
                }
            }

            if (buttons > 0)
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }
            
            DrawDefaultInspector();
            
            if (componentDefinitionsFromObjects.Count > 0)
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                // foreach component render inside this one + remove button

                foreach (var componentDefinition in componentDefinitionsFromObjects)
                {
                    // componentDefinition.hideFlags = HideFlags.None;

                    if (objectEntityDefinition.hideMonoBehaviours)
                    {
                        componentDefinition.hideFlags = HideFlags.HideInInspector;
                    }
                    else
                    {
                        componentDefinition.hideFlags = HideFlags.None;
                    }
                }

                if (objectEntityDefinition.hideMonoBehaviours)
                {
                    // EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    componentDefinitionsFromObjects.Sort(delegate(ComponentDefinitionBase a, ComponentDefinitionBase b)
                    {
                        return string.Compare(a.GetType().Name, b.GetType().Name,
                            StringComparison.OrdinalIgnoreCase);
                    });

                    var copyOfDefinitions = new List<ComponentDefinitionBase>(componentDefinitionsFromObjects);
                    foreach (var componentDefinition in copyOfDefinitions)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField($"<< {componentDefinition.GetComponentName()} >>", style);

                        var remove = GUILayout.Button("Remove");
                        
                        if (remove)
                        {
                            if (EditorUtility.DisplayDialog("Confirm",
                                    "This will remove the component and lose serialized data with it", "Ok", "Cancel"))
                            {
                                GameObject.DestroyImmediate(componentDefinition);
                            }
                        }

                        EditorGUILayout.EndHorizontal();

                        if (!remove)
                        {
                            var serializedObject = new SerializedObject(componentDefinition);
                            CustomEditorExtensions.DrawInspectorExcept(serializedObject, new[] { "m_Script" });
                            // EditorGUILayout.Separator();
                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                        }

                    }
                }
            }
            
            EditorGUILayout.EndVertical();
        }
    }
}