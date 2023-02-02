using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gemserk.BuildTools.Editor
{
    public class BuildConfigurationsWindow : EditorWindow
    {
        private List<BuildConfiguration> buildConfigurations = new List<BuildConfiguration>();
        private bool[] foldouts = new bool[100];

        [MenuItem("Window/Gemserk/Build Configurations")]
        public static void ShowWindow()
        {
            // This method is called when the user selects the menu item in the Editor
            EditorWindow wnd = GetWindow<BuildConfigurationsWindow>();
            wnd.titleContent = new GUIContent("Build Configurations");
        }

        private void OnFocus()
        {
            var buildConfigurationGuids = AssetDatabase.FindAssets($"t:{typeof(BuildConfiguration)}");
            buildConfigurations = buildConfigurationGuids
                .Select(g => AssetDatabase.LoadAssetAtPath<BuildConfiguration>(AssetDatabase.GUIDToAssetPath(g)))
                .ToList();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            for (var i = 0; i < buildConfigurations.Count; i++)
            {
                var buildConfiguration = buildConfigurations[i];
                foldouts[i] = EditorGUILayout.Foldout(foldouts[i], buildConfiguration.name);
                if (foldouts[i])
                {
                    EditorGUI.indentLevel++;
                    var objectEditor = UnityEditor.Editor.CreateEditor(buildConfiguration);
                    objectEditor.OnInspectorGUI();

                    // EditorGUILayout.BeginHorizontal();
                    // if (GUILayout.Button(new GUIContent("Load", null, "Overwrite current ProjectSettings with saved configuration asset.")))
                    // {
                    //     buildConfiguration.Load();
                    // }
                    //
                    // if (GUILayout.Button(new GUIContent("Store", null, "Stores current ProjectSettings in configuration asset.")))
                    // {
                    //     if (EditorUtility.DisplayDialog("Warning", "Overwrite asset with current Editor Settings?", "Ok", "Cancel"))
                    //     {
                    //         buildConfiguration.Store();
                    //     }
                    // }
                    // EditorGUILayout.EndHorizontal();
                    
                    EditorGUI.indentLevel--;
                }
            }

            EditorGUILayout.EndVertical();
        }
    }
}