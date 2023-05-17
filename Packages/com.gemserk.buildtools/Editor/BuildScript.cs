using System;
using System.IO;
using System.Linq;
using Gemserk.RefactorTools.Editor;
using UnityEditor;
using UnityEngine;

namespace Gemserk.BuildTools.Editor
{
    public class BuildScript
    {
        private const string GemserkProjectName = "GEMSERK_PROJECT_NAME";
        private const string GemserkGameBuildPath = "GEMSERK_GAME_BUILD_PATH";

        public static void BuildWebGL()
        {
            Build(new BuildPlayerOptions
            {
                target = BuildTarget.WebGL,
                targetGroup = BuildTargetGroup.WebGL,
                options = BuildOptions.None
            });
        }
        
        public static void BuildWindows()
        {
            Build(new BuildPlayerOptions
            {
                target = BuildTarget.StandaloneWindows64,
                targetGroup = BuildTargetGroup.Standalone,
                options = BuildOptions.None
            });
        }
        
        public static void BuildLinux()
        {
            Build(new BuildPlayerOptions
            {
                target = BuildTarget.StandaloneLinux64,
                targetGroup = BuildTargetGroup.Standalone,
                options = BuildOptions.None
            });
        }
        
        public static void BuildMacos()
        {
            Build(new BuildPlayerOptions
            {
                target = BuildTarget.StandaloneOSX,
                targetGroup = BuildTargetGroup.Standalone,
                options = BuildOptions.None
            });
        }

        private static void OverrideVersionTextResources(string version)
        {
            // var versionText = AssetDatabaseExt.FindAssets<TextAsset>();
            // var versionTextAsset = AssetDatabase.LoadAssetAtPath<TextAsset>("Resources/version.txt");
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmm");
            File.WriteAllText("Resources/version.txt", $"{version}_{timestamp}");
            // AssetDatabase.Refresh();
        }
        
        public static void Build(BuildPlayerOptions buildOptions)
        {
            var projectName = Environment.GetEnvironmentVariable(GemserkProjectName);
            var buildPath = Environment.GetEnvironmentVariable(GemserkGameBuildPath);

            if (string.IsNullOrEmpty(projectName))
            {
                Debug.Log($"BUILD FAILED: missing {GemserkProjectName}");
                EditorApplication.Exit(1);
            }
            
            if (string.IsNullOrEmpty(buildPath))
            {
                Debug.Log($"BUILD FAILED: missing {GemserkGameBuildPath}");
                EditorApplication.Exit(1);
            }

            var buildConfiguration = 
                AssetDatabaseExt.FindAssets<BuildConfiguration>().FirstOrDefault(b => 
                    b.name.Equals(projectName, StringComparison.OrdinalIgnoreCase));

            if (buildConfiguration == null)
            {
                Debug.Log($"BUILD FAILED: missing build configuration named {projectName}");
                EditorApplication.Exit(1);
            }
            
            buildConfiguration.Load();

            OverrideVersionTextResources(buildConfiguration.version);

            buildOptions.locationPathName = buildPath;
            buildOptions.scenes = EditorBuildSettings.scenes.Select(s => s.path).ToArray();

            var buildReport = BuildPipeline.BuildPlayer(buildOptions);

            if (buildReport.summary.totalErrors > 0)
            {
                Debug.Log(JsonUtility.ToJson(buildReport, true));
                EditorApplication.Exit(1);
            }
            
            EditorApplication.Exit(0);
        }
    }
}