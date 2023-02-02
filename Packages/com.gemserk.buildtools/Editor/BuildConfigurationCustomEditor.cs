using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gemserk.BuildTools.Editor
{
    [CustomEditor(typeof(BuildConfiguration), true)]
    public class BuildConfigurationCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var buildConfiguration = target as BuildConfiguration;
            
            if (GUILayout.Button(new GUIContent("Load", null, "Load config in project settings")))
            {
                EditorBuildSettings.scenes =
                    buildConfiguration.sceneAssets.Select(s => new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(s), true))
                        .ToArray();
            }

            if (GUILayout.Button(new GUIContent("Read current", null, "Overwrite with current project settings")))
            {
                // show confirmation dialog just in case
                buildConfiguration.sceneAssets = EditorBuildSettings.scenes
                    .Select(s => AssetDatabase.LoadAssetAtPath<SceneAsset>(s.path)).ToList();

                EditorUtility.SetDirty(buildConfiguration);
            }
        }
    }
}