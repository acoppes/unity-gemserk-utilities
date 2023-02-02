using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gemserk.BuildTools.Editor
{
    [CustomEditor(typeof(BuildConfiguration), true)]
    public class BuildConfigurationCustomEditor : UnityEditor.Editor
    {
        private void CopyFile(string sourceFolder, string destinationFolder, string fileName)
        {
            var sourceFile = Path.Combine(sourceFolder, fileName);
            var destinationFile = Path.Combine(destinationFolder, fileName);

            if (File.Exists(destinationFile))
            {
                File.Delete(destinationFile);
            }
            
            
            FileUtil.CopyFileOrDirectory(sourceFile, destinationFile);
            
            AssetDatabase.Refresh();
        }
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var buildConfiguration = target as BuildConfiguration;

            // var disabled = string.IsNullOrEmpty(buildConfiguration.settingsFolder);
            
            // EditorGUI.BeginDisabledGroup(disabled);
            
            if (GUILayout.Button(new GUIContent("Load Saved Settings", null, "Load config in project settings")))
            {
                // var sourceFolder = Path.Combine(Application.dataPath, buildConfiguration.settingsFolder);
                // var destinationFolder = Path.Combine(Application.dataPath, "../ProjectSettings");
                //
                // CopyFile(sourceFolder, destinationFolder, "ProjectSettings.asset");
                // CopyFile(sourceFolder, destinationFolder, "EditorBuildSettings.asset");

                EditorBuildSettings.scenes =
                    buildConfiguration.sceneAssets.Select(s => new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(s), true))
                        .ToArray();

                PlayerSettings.productName = buildConfiguration.productName;
                PlayerSettings.defaultWebScreenHeight = buildConfiguration.defaultWebScreenHeight;
                PlayerSettings.defaultWebScreenWidth = buildConfiguration.defaultWebScreenWidth;
            }

            if (GUILayout.Button(new GUIContent("Save From Current Settings", null, "Overwrite with current project settings")))
            {
                if (EditorUtility.DisplayDialog("Warning", "Overwrite asset with current Editor Settings?", "Ok", "Cancel"))
                {
                    // var sourceFolder = Path.Combine(Application.dataPath, "../ProjectSettings");
                    // var destinationFolder = Path.Combine(Application.dataPath, buildConfiguration.settingsFolder);
                    //
                    // CopyFile(sourceFolder, destinationFolder, "ProjectSettings.asset");
                    // CopyFile(sourceFolder, destinationFolder, "EditorBuildSettings.asset");
                    
                    buildConfiguration.productName = PlayerSettings.productName;
                    buildConfiguration.defaultWebScreenHeight = PlayerSettings.defaultWebScreenHeight;
                    buildConfiguration.defaultWebScreenWidth = PlayerSettings.defaultWebScreenWidth;
                    
                    // // show confirmation dialog just in case
                    buildConfiguration.sceneAssets = EditorBuildSettings.scenes
                        .Select(s => AssetDatabase.LoadAssetAtPath<SceneAsset>(s.path)).ToList();
                    
                    EditorUtility.SetDirty(buildConfiguration);
                }
            }
            
            // EditorGUI.EndDisabledGroup();
        }
    }
}