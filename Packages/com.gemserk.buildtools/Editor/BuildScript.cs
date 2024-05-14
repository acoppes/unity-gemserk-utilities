using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Gemserk.RefactorTools.Editor;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using Debug = UnityEngine.Debug;

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
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmm");

            if (!Directory.Exists("Assets/Resources"))
            {
                Directory.CreateDirectory("Assets/Resources");
            }
            
            File.WriteAllText("Assets/Resources/version.txt", $"{version}_{timestamp}");
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

            var timerBuild = Stopwatch.StartNew();

            var buildReport = BuildPipeline.BuildPlayer(buildOptions);
            
            var buildSummary = new StringBuilder();
            buildSummary.AppendLine("BUILDSUMMARY");
            buildSummary.AppendFormat("Build: {0}\n", timerBuild.Elapsed.TotalSeconds);

            buildSummary.AppendLine("-----------");
            buildSummary.AppendLine("BUILD REPORT");
            buildSummary.AppendLine("-----------");
            
            foreach (var buildReportStep in buildReport.steps)
            {
                buildSummary.AppendFormat("Step: {0} - Duration: {1}\n", buildReportStep.name,
                    buildReportStep.duration.TotalSeconds);
            }
            
            Debug.Log(buildSummary.ToString());

            if (buildReport.summary.result == BuildResult.Failed)
            {
                var errorReport = new StringBuilder();
                
                foreach (var step in buildReport.steps)
                {
                    foreach (var message in step.messages)
                    {
                        if (message.type == LogType.Error || message.type == LogType.Exception || message.type == LogType.Assert)
                        {
                            errorReport.AppendLine($"ERROR: step:{step.name} - {message.content}");
                        }
                    }
                }

                var errorMessage = $"Build Failed with :{buildReport.summary.totalErrors} errors";
                Debug.Log($"{errorMessage}\n{errorReport}");
                
                EditorApplication.Exit(1);
            }
            
            EditorApplication.Exit(0);
        }
    }
}